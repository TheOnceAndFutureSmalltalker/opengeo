import React, { Component } from 'react';
import L from 'leaflet';
import 'leaflet/dist/leaflet.css';
import { connect } from 'react-redux';
import { createMainMap } from '../actions'

class OpenGeoMap extends Component {
	constructor(props) {
		super(props);
		this.map = null;
	}

	componentDidUpdate() {
		if (!this.map && this.props.map_parameters ) {
			this.map = L.map('map', {
				center: [this.props.map_parameters.lat, this.props.map_parameters.lng],
				zoom: this.props.map_parameters.zoom,
				zoomControl: false
			});
			this.props.createMainMap(this.map);
        }
	}


	render() {
		return (
			(this.props.map_parameters)
			? <div id='map' style={{ width: '100%', height: this.props.height || '100%' }} />
			: null
			);
	}
}

const mapStateToProps = (state) => {
	return {
		map_parameters: state.opengeo.map_parameters
    }
};

export default connect(mapStateToProps, { createMainMap })(OpenGeoMap);