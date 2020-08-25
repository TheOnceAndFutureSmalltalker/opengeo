
import React, { Component } from 'react';
import axios from 'axios';
import FuseSplashScreen from '@fuse/core/FuseSplashScreen';
import { connect } from 'react-redux';
import { accessedBasemapList } from '../actions'


class OpenGeoStaticData extends Component {
	constructor(props) {
		super(props);
		this.state = { loading: true };
	}

	componentDidMount() {
		this.getBasemapList();
	}

	getBasemapList = async () => {
		const response = await axios.get('/api/basemaps/');
		const basemaps = response.data;
		this.props.accessedBasemapList(basemaps);
		this.setState({ loading: false });
	};

    render() {
		return this.state.loading ? <FuseSplashScreen /> : <>{this.props.children}</>;
    }
}


export default connect(null, { accessedBasemapList })(OpenGeoStaticData);

 