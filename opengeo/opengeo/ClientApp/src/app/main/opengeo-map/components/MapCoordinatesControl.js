import React, { Component } from 'react';
import Tooltip from '@material-ui/core/Tooltip';
import Grid from '@material-ui/core/Grid';
import Typography from '@material-ui/core/Typography';
import { connect } from 'react-redux';



class MapCoordinatesControl extends Component {
	constructor(props) {
		//console.log("create MapCoordinatesControl");
		super(props);
		this.coordinates = 'decimal';
		this.state = { lat: 0, lng: 0 };
		this.handlersRegistered = false;
	}

	componentWillUnmount() {
		if (this.handlersRegistered) {
			this.map.off('mousemove', this.onMapMouseMove);
		}
	}

	onMapMouseMove = (e) => {
		if (this.coordinates === 'degrees') {
			let lat = this.convertDecimalLatToDegrees(e.latlng.lat);
			let lng = this.convertDecimalLngToDegrees(e.latlng.lng);
			this.setState({ lat: lat, lng: lng });
		} else {
			let lat = e.latlng.lat.toLocaleString('en-US', { minimumFractionDigits: 4, useGrouping: false });
			let lng = e.latlng.lng.toLocaleString('en-US', { minimumFractionDigits: 4, useGrouping: false });
			this.setState({ lat: lat, lng: lng });
		}
	};

	convertDecimalLatToDegrees(lat) {
		var dms = this.convertDDToDMS(lat, false);
		var dmsDeg = dms.deg.toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false });
		var dmsMin = dms.min.toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false });
		var dmsSec = dms.sec.toLocaleString('en-US', { minimumIntegerDigits: 2, minimumFractionDigits: 2, useGrouping: false });
		var dmsString = dmsDeg + 'º ' + dmsMin + '′ ' + dmsSec + '′′ ' + dms.dir;
		return dmsString;
	}

	convertDecimalLngToDegrees(lng) {
		var dms = this.convertDDToDMS(lng, true)
		var dmsDeg = dms.deg.toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false });
		var dmsMin = dms.min.toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false });
		var dmsSec = dms.sec.toLocaleString('en-US', { minimumIntegerDigits: 2, minimumFractionDigits: 2, useGrouping: false });
		var dmsString = dmsDeg + 'º ' + dmsMin + '′ ' + dmsSec + '′′ ' + dms.dir;
		return dmsString;
	}

	convertDDToDMS(D, lng) {
		return {
			dir: D < 0 ? lng ? 'W' : 'S' : lng ? 'E' : 'N',
			deg: 0 | (D < 0 ? D = -D : D),
			min: 0 | D % 1 * 60,
			sec: (0 | D * 60 % 1 * 6000) / 100
		};
	}

	shouldComponentUpdate(nextProps, nextState) {
		if (!this.handlersRegistered && nextProps.map) {
			nextProps.map.on('mousemove', this.onMapMouseMove, this);
			this.handlersRegistered = true;
			return false;
		}
		return true;
	}

	render() {
		return (
			(this.props.map)
				?
				<Grid container spacing={1} alignItems="center">
					<Grid item key={1}>
						<Tooltip title="Mouse Coordinates">
							<Typography variant="h5" align='right' style={{ color: '#656565', marginBottom: '2px' }}>[{this.state.lat},&nbsp;{this.state.lng}]</Typography>
						</Tooltip >
					</Grid>
				</Grid>
				:
				null
		);
	}
}


const mapStateToProps = (state) => {
	return {
		map: state.opengeo.map
	}
};

export default connect(mapStateToProps)(MapCoordinatesControl);
