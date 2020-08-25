import React, { Component } from 'react';
import { IconButton } from "@material-ui/core";
import Tooltip from '@material-ui/core/Tooltip';
import Grid from '@material-ui/core/Grid';
import Typography from '@material-ui/core/Typography';
import ZoomIn from "@material-ui/icons/ZoomIn";
import ZoomOut from "@material-ui/icons/ZoomOut";
import { connect } from 'react-redux';
import { changedMapParameters } from '../actions'


class MapZoomControl extends Component {
	constructor(props) {
		super(props);
		this.handlersRegistered = false;
		this.state = { zoom: 0 };
	}

	componentWillUnmount() {
		if (this.handlersRegistered) {
			this.map.off('zoomend', this.onMapZoomEnd, this);
			this.map.off('moveend', this.onMapMoveEnd, this);
        }
	}

	onMapZoomEnd = (e) => {
		this.setState({ zoom: this.props.map.getZoom() });
		this.updateMapParameters();
	};

	// updates map_parameters in store with zoom/pan changes
	updateMapParameters(e) {
		const center = this.props.map.getCenter();
		const zoom = this.props.map.getZoom();
		const id = this.props.map._container.id;
		const map_parameters = { id, zoom, lat: center.lat, lng: center.lng };
		this.props.changedMapParameters(map_parameters);
    }

	onMapMoveEnd = (e) => {
		this.updateMapParameters();
	};

	zoomIn = () => {
		this.props.map.zoomIn();
	};

	zoomOut = () => {
		this.props.map.zoomOut();
	};

	shouldComponentUpdate(nextProps, nextState) {
		if (!this.handlersRegistered && nextProps.map) {
			nextProps.map.on('zoomend', this.onMapZoomEnd, this);
			nextProps.map.on('moveend', this.onMapMoveEnd, this);
			this.handlersRegistered = true;
			this.setState({ zoom: nextProps.map.getZoom() });
			return false;
		}
		return true;
    }

	render() {
		return (
			(this.props.map)
			? <React.Fragment>
				<Grid container spacing={1} alignItems="center">
					<Grid item key={1}>
						<div>
							<Tooltip title="Zoom In">
								<IconButton size="small"
									onClick={() => this.zoomIn()}>
									<ZoomIn fontSize="large" />
								</IconButton>
							</Tooltip>

							<Tooltip title="Zoom Out">
								<IconButton size="small"
									onClick={() => this.zoomOut()}>
									<ZoomOut fontSize="large" />
								</IconButton>
							</Tooltip>
						</div>
					</Grid>

					<Grid item key={2} >
						<Tooltip title="Zoom Level">
							<Typography variant="h5" style={{ color: '#656565', marginBottom: '0px' }}>{this.state.zoom}</Typography>
						</Tooltip>
					</Grid>

				</Grid>

			</React.Fragment>
			: null
		);
	}
}


const mapStateToProps = (state) => {
	return {
		map: state.opengeo.map
	}
};

export default connect(mapStateToProps, { changedMapParameters })(MapZoomControl);

