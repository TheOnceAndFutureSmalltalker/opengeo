import React from 'react'
import Grid from '@material-ui/core/Grid';
import { IconButton } from "@material-ui/core";
import Tooltip from '@material-ui/core/Tooltip';

import { connect } from 'react-redux';
import L from 'leaflet';
import 'app/components/leaflet_draw';
import 'app/components/leaflet_draw.css';

import AddIcon from "@material-ui/icons/Add";
import EditIcon from "@material-ui/icons/Edit";
import DeleteIcon from "@material-ui/icons/Delete";
import CancelIcon from "@material-ui/icons/Cancel";
import UndoIcon from "@material-ui/icons/Undo";
import DoneIcon from "@material-ui/icons/Done";

const polyline_options = {
	shapeOptions: {
		color: 'blue',
		weight: 2,
		opacity: 1.0
	}
};

const polygon_options = {
	shapeOptions: {
		color: 'blue',
		weight: 2,
		opacity: 1.0
	}
};

const marker_options = {
	icon: {
		iconUrl: 'http://localhost:8080/geoserver/www/img/marker-icon.png',
		iconSize: [25, 41],
		iconAnchor: [12, 41],
		popupAnchor: [1, -34],
		tooltipAnchor: [16, -28]//,
		//shadowUrl: 'my-icon-shadow.png',
		//shadowSize: [68, 95],
		//shadowAnchor: [22, 94]
	}
};


class FeatureEditingTool extends React.Component {
	constructor(props) {
		super(props);
		this.geometryType = "polygon"; // polygon, polyline, marker
		this.drawControl = null;
		this.editing = null;
		this.state = { operation: null };
	}

	addFeature = () => {
		if (this.geometryType === "polygon") {
			this.editing = new L.Draw.Polygon(this.props.map, polygon_options);
			this.editing.enable();
		} else if (this.geometryType === "polyline") {
			this.editing = new L.Draw.Polyline(this.props.map, polyline_options);
			this.editing.enable();
		} else if (this.geometryType === "marker") {
			this.editing = new L.Draw.Marker(this.props.map, null);
			this.editing.enable();
		}
		this.setState({ operation: 'add' });
	};

	finishFeature = () => {
		if (this.geometryType === 'polygon' || this.geometryType === 'polyline') {
			this.editing._finishShape();
			this.editing = null;
			this.operation = null;
        }
	};

	editFeature = () => {
		this.editing = new L.EditToolbar.Edit(this.props.map,
			{
				featureGroup: this.props.layer_being_edited.leaflet,
				selectedPathoptions: this.drawControl.options.edit.selectedPathOptions});
		this.editing.enable();
		this.setState({ operation: 'edit' });
	};

	removeFeature = () => {
		this.editing = new L.EditToolbar.Delete(this.props.map, { featureGroup: this.props.layer_being_edited.leaflet });
		this.editing.enable();
		this.setState({ operation: 'delete' });
	};

	acceptChanges = () => {
		if (this.state.operation === 'add' && (this.geometryType === 'polygon' || this.geometryType === 'polyline')) {
			this.editing._finishShape();
		} else {
			this.editing.save();
			this.editing.disable();
		}
		this.editing = null;
		this.setState({ operation: null });
	};

	cancelChanges = () => {
		if (this.state.operation === 'edit' || this.state.operation === 'delete') {
			this.editing.revertLayers();
		}
		this.editing.disable();
		this.editing = null;
		this.setState({ operation: null });
	};


	shouldComponentUpdate(nextProps, nextState) {
		// first, we need a map to exist!!!
		if (this.props.map) {
			// if going from a layer for layer_being_edited to null, then stop editing
			if (this.props.layer_being_edited && !nextProps.layer_being_edited) {
				if (this.drawControl) {
					this.props.map.off(L.Draw.Event.CREATED);
					this.props.map.off(L.Draw.Event.EDITED);
					this.props.map.off(L.Draw.Event.DELETED);
					this.props.map.removeControl(this.drawControl);
					this.drawControl = null;
				}
			}
			// if going from null layer_being_edited to actual layer, then start editing
			if (!this.props.layer_being_edited && nextProps.layer_being_edited) {
				this.drawControl = new L.Control.Draw({
					draw: false,
					edit: {
						featureGroup: nextProps.layer_being_edited.leaflet,
						edit: false,
						remove: false
					},
					delete: false
				});

				let self = this;

				this.props.map.on(L.Draw.Event.CREATED, function (event) {
					console.log('L.Draw.Event.CREATED');
					var layer = event.layer;
					self.props.layer_being_edited.leaflet.addLayer(layer);
					if (self.editing) {
						self.editing = null;
						self.setState({ operation: null });
                    }
				});

				this.props.map.on(L.Draw.Event.EDITED, function (event) {
					console.log('L.Draw.Event.EDITED');
					var layers = event.layers;
					self.props.layer_being_edited.leaflet.saveLayers(layers);
				});

				this.props.map.on(L.Draw.Event.DELETED, function (event) {
					console.log('L.Draw.Event.DELETED');
					var layers = event.layers;
					self.props.layer_being_edited.leaflet.removeLayers(layers);
					
				});

				this.props.map.addControl(this.drawControl);
				this.geometryType = nextProps.layer_being_edited.geometryType;
			}
		}
		return true;
	}


	render() {
		return (
			this.props.layer_being_edited
				? <Grid container spacing={1} alignItems="center">
					{!this.state.operation &&
						<React.Fragment>
						<Grid item key={1}>
							<Tooltip title={"Add " + this.geometryType}>
								<IconButton size="small"
									onClick={() => this.addFeature()}>
									<AddIcon fontSize="large" />
								</IconButton>
							</Tooltip>
						</Grid>
						<Grid item key={4}>
							<Tooltip title={"Edit " + this.geometryType}>
								<IconButton size="small"
									onClick={() => this.editFeature()}>
									<EditIcon fontSize="large" />
								</IconButton>
							</Tooltip>
						</Grid>
						<Grid item key={5}>
							<Tooltip title={"Remove " + this.geometryType}>
								<IconButton size="small"
									onClick={() => this.removeFeature()}>
									<DeleteIcon fontSize="large" />
								</IconButton>
							</Tooltip>
						</Grid>
						</React.Fragment>
					}
					{this.state.operation &&
						<React.Fragment>
						<Grid item key={2}>
							<Tooltip title={"Finish " + this.geometryType}>
								<IconButton size="small"
									onClick={() => this.acceptChanges()}>
									<DoneIcon fontSize="large" />
								</IconButton>
							</Tooltip>
						</Grid>
						<Grid item key={6}>
							<Tooltip title="Cancel">
								<IconButton size="small"
									onClick={() => this.cancelChanges()}>
									<CancelIcon fontSize="large" />
								</IconButton>
							</Tooltip>
						</Grid>
						</React.Fragment>
					}
				</Grid>
				: null
		);

    }
}

const mapStateToProps = (state) => {
	return {
		map: state.opengeo.map,
		layer_being_edited: state.opengeo.layer_being_edited
	}
};

export default connect(mapStateToProps)(FeatureEditingTool);