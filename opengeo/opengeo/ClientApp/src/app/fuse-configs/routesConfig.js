import React from 'react';
import { Redirect } from 'react-router-dom';
import FuseUtils from '@fuse/utils';
import ExampleConfig from 'app/main/example/ExampleConfig';
import OpenGeoMapPage from 'app/main/opengeo-map/OpenGeoMapPage';

const routeConfigs = [ExampleConfig];

const routes = [
	...FuseUtils.generateRoutesFromConfigs(routeConfigs),
	{
		path: '/opengeo-map/:id',
		component: OpenGeoMapPage
	},
	{
		path: '/',
		component: () => <Redirect to="/example" />
	}
];

export default routes;
