import React, { Component } from 'react';
import ListItemText from "@material-ui/core/ListItemText";
import List from "@material-ui/core/List";
import ListItem from "@material-ui/core/ListItem";
import { IconButton } from "@material-ui/core";
import ListItemIcon from "@material-ui/core/ListItemIcon";
import EditIcon from "@material-ui/icons/Edit";
import Checkbox from "@material-ui/core/Checkbox";
import Paper from "@material-ui/core/Paper";
import L from 'leaflet';
import 'app/components/leaflet_rest';
import { connect } from 'react-redux';
import { selectedLayerForEditing, updatedLayers } from '../actions'


function getStyles(features, styles) {
	if (styles.length > 0) {
		return styles[0];
	} else {
		const styles = { color: 'blue', weight: 2 };
		return styles;
	}
}

class MapLayersList extends Component {
	constructor(props) {
		super(props);
		this.initialized = false;
	}

	pointToLayer = (feature, latlng) => {
		return L.marker(latlng);
	};

	
	toggleLayer = (e, layer) => {

		if (layer.showing) {
			layer.showing = false;
			this.props.map.removeLayer(layer.leaflet);
		} else {
			layer.showing = true;
			this.props.map.addLayer(layer.leaflet);
		}
		let layers = [ ...this.props.layers ];
		this.props.updatedLayers(layers);	
		if (layer === this.props.layer_being_edited) {
			this.props.selectedLayerForEditing(null);
        }
	};




	shouldComponentUpdate(nextProps, nextState) {
		if (!this.initialized && nextProps.layers && nextProps.map) {
			let layers = [];
			nextProps.layers.forEach(layer => {
				
				let options = {
					url: layer.url,
					//pointToLayer: this.pointToLayer,
					primaryKeyField: 'fid',
					crs: L.CRS.EPSG4326,
					style: function (features) { return getStyles(features, layer.layerStyles);},
					showToolbar: false
				};
				layer.leaflet = new L.REST(options);
				layer.showing = false;
				layers.push(layer);
			});
			this.props.updatedLayers(layers);
			this.initialized = true;
			return true;
		}
		return true;
	}

	editLayer = (e, layer) => {
		if (layer === this.props.layer_being_edited) {
			this.props.selectedLayerForEditing(null);
		} else {
			if (this.props.layer_being_edited) return;
			if (!layer.showing) return;
			this.props.selectedLayerForEditing(layer);
        }
	};
	

	render() {

		return (
			(this.props.layers && this.props.map)
				? <div >
					<h2 style={{ padding: 5, paddingLeft: 10 }}>Layers</h2>
					<Paper style={{ height: '100%', overflow: 'auto' }} >
						<List>
							{this.props.layers.map(l => {
								return (
									<React.Fragment key={l.name}>
										<ListItem>
											<ListItemIcon size='small' >
												<React.Fragment>
												<Checkbox
													onClick={(e) => this.toggleLayer(e, l)}
													edge="start"
													checked={l.showing}
													size='small'
												/>
												<IconButton
													size="small"
													disabled={!l.showing}
													onClick={(e) => { this.editLayer(e, l) }}>
													<EditIcon style={(this.props.layer_being_edited === l) ? { color: '#61dafb' } : {color: '#CCCCCC'}}/>
												</IconButton>
												</React.Fragment>
											</ListItemIcon>
											<ListItemText primary={l.name} size='small' />
										</ListItem>
									</React.Fragment>
								);
							})}
						</List>
						</Paper>
				</div>
				: <div></div>
		);
	}
}

const mapStateToProps = (state) => {
	return {
		map: state.opengeo.map,
		layers: state.opengeo.layers,
		layer_being_edited: state.opengeo.layer_being_edited
	}
};

export default connect(mapStateToProps, { selectedLayerForEditing, updatedLayers })(MapLayersList);
