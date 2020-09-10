import React, { Component } from 'react';
import OpenGeoMapToolbar from './components/OpenGeoMapToolbar';
import OpenGeoMap from './components/OpenGeoMap';
import OpenGeoMapNavigator from './components/OpenGeoMapNavigator';
import FeatureAttributesControl from './components/FeatureAttributesControl';
import axios from 'axios';
import { connect } from 'react-redux';
import { accessedMapParameters, accessedBasemap, accessedLayers } from './actions'
import SplitPane from 'react-split-pane';


class OpenGeoMapPage extends Component {
	constructor(props) {
		//console.log("create OpenGeoMapPage");
		super(props);
		// id for the API call for map data comes in on the url
		this.map_id = props.match.params.id;
		this.rightHandRef = React.createRef();
	}

	componentDidMount() {
		this.getMapData();
	}

	getMapData = async () => {
		const response = await axios.get('/api/maps/' + this.map_id);
		const map = response.data;
		this.props.accessedMapParameters({id: map.id, zoom: map.zoom, lat: map.centerLat, lng: map.centerLong});
		this.props.accessedBasemap(map.basemap);
		this.props.accessedLayers(map.layer.filter((l) => { return l.format === 'geojson'})); // map.layer is actually an array of layer objects!
	};


	splitPaneChanged = (e) => {
		console.log(e);
		//let self = this;
		//setTimeout(function () { self.props.map.invalidateSize() }, 400);
	};

	getMapHeight = () => {
		const rightHandPane = this.rightHandRef.current;
		if (rightHandPane) {
			const height = rightHandPane.clientHeight - 41;
			return height.toString() + 'px';
        }
		return null;
	};

	render() {
		return (
			<SplitPane split="vertical" minSize={150} defaultSize={300} onDragFinished={(e) => { this.splitPaneChanged(e); }}>
				<OpenGeoMapNavigator />
				<div id='right-pane' style={{ width: '100%', height: '100%' }} ref={this.rightHandRef} >
					<OpenGeoMapToolbar />
					<OpenGeoMap height={this.getMapHeight()}/>
					<FeatureAttributesControl />
				</div>
			</SplitPane>
			
		);
	}
}

const mapStateToProps = (state) => {
	return {
		map: state.opengeo.map
	}
};

export default connect(mapStateToProps, { accessedMapParameters, accessedBasemap, accessedLayers })(OpenGeoMapPage);
