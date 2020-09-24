import React from 'react';
import OpenGeoMapPage from './OpenGeoMapPage';

const OpenGeoMapPageConfig = {
	settings: {
		layout: {
			config: {}
		}
	},
	routes: [
		{
			path: '/opengeo-map/:id',
			component: OpenGeoMapPage
		}
	]
};

export default OpenGeoMapPageConfig;
