import { makeStyles } from '@material-ui/core/styles';
import React, { useRef } from 'react';
import L from "leaflet";
import SplitPane from 'react-split-pane';


const useStyles = makeStyles({
	layoutRoot: {}
});


class ExamplePage extends React.Component {
	constructor(props) {
		//console.log("create OpenGeoMapPage");
		super(props);
		// id for the map comes in on the url
		this.map_id = props.match.params.id;
		this.state = { loading: true };
		this.map = null;
	}

	componentDidMount() {
		this.map = L.map('map', {
			center: [40.78, -73.96],
			zoom: 11
		});
		const url = 'https://basemap.nationalmap.gov:443/arcgis/services/USGSTopo/MapServer/WmsServer?';
		const basemap = L.tileLayer.wms(url, { layers: '0' });
		this.map.addLayer(basemap);
	}

	resizeMap = (e) => {
		console.log(e);
		let self = this;
		setTimeout(function () { self.map.invalidateSize() }, 400);
	};


	render() {
		return (
			<SplitPane split="vertical" minSize={150} defaultSize={300} onDragFinished={(e) => { this.resizeMap(e);}}>
				<div />
				<div id='map' style={{ width: '100%', height: '100%' }}>
					
				</div>
			</SplitPane>
		);
    }
};



export default ExamplePage;