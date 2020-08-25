import React, { Component } from 'react';
import Grid from '@material-ui/core/Grid';
import MapCoordinatesControl from './MapCoordinatesControl';
import MapZoomControl from './MapZoomControl';
import FeatureEditingTool from './FeatureEditingTool';


class OpenGeoMapToolbar extends Component {
	constructor(props) {
		//console.log("create OpenGeoMapToolbar");
		super(props);
	}


	render() {
		return (
			<Grid container justify="space-between" alignItems="center">
				<Grid item value={1}>
					<Grid container spacing={2} >
						<Grid item value={1}>
						</Grid>
						<Grid item value={2}>
							<MapZoomControl map_id={this.props.map_id}/>
						</Grid>
						<Grid item value={2}>
							<FeatureEditingTool />
						</Grid>
						
					</Grid>
				</Grid>

				<Grid item value={2}>
					<Grid container spacing={3}>
						<Grid item value={1}>
							<MapCoordinatesControl map_id={this.props.map_id}/>
						</Grid>
						<Grid item value={2}>
						</Grid>
					</Grid>
				</Grid>
				
			</Grid>

			);
	}
}

export default OpenGeoMapToolbar;