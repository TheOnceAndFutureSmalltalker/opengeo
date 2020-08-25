import React, { Component } from 'react';
import MapLayersList from "./MapLayersList"
import BasemapList from './BasemapList'


class OpenGeoMapNavigator extends Component {
	constructor(props) {
		//console.log("create OpenGeoMapNavigator");
		super(props);
	}


	render() {
		return (
			<div >
				<BasemapList height={300} />
				<MapLayersList  />
			</div>
		);
	}
}


export default OpenGeoMapNavigator;