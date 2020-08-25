import React from 'react';
import Button from '@material-ui/core/Button';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import DialogTitle from '@material-ui/core/DialogTitle';
import { makeStyles } from '@material-ui/core/styles';
import Popper from '@material-ui/core/Popper';
import Typography from '@material-ui/core/Typography';
import Fade from '@material-ui/core/Fade';
import Paper from '@material-ui/core/Paper';
import MaUTable from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import { connect } from 'react-redux';
import L from 'leaflet';

import axios from 'axios';

const useStyles = makeStyles((theme) => ({
    typography: {
        padding: theme.spacing(2),
    },
}));

class FeatureAttributesControl extends React.Component {
	constructor(props) {
		super(props);
		this.initialized = false;
        this.state = { selectedFeature: null };
	}

	handleMapClick = (e) => {
		if (!this.props.layers) return;
		//console.log('Clicked Map');
		// search for feature hit
		// if found set dispaly state
		// open dialog 
        // set up map click handler that queries for GetFeatureInfo
        let map = this.props.map;
        let containerPoint = e.containerPoint;
        let containerLatLng = map.layerPointToLatLng(containerPoint);
        let selectedFeatures = [];
        // if event did not originate with a leaflet object, just return
        //if (!this.isLeafletEvent(e)) return;

        this.props.layers
            .filter((layer) => { return layer.showing; })
            .forEach((layer) => {
                let features = layer.leaflet._layers;
                Object.values(features).forEach((feature) => {
                    if (feature instanceof L.Marker) {
                        let featurePoint = map.latLngToLayerPoint(feature._latlng);
                        if (Math.abs(containerPoint.x - featurePoint.x) < 20
                            && (featurePoint.y - containerPoint.y) > 0
                            && (featurePoint.y - containerPoint.y) <= 40) {
                            selectedFeatures.push(feature);
                        }
                    } else if (feature instanceof L.Polygon) {
                        var polyPoints = feature.getLatLngs()[0];
                        if (feature.feature.geometry.type == 'MultiPolygon') {
                            polyPoints = polyPoints[0];
                        }
                        var x = containerLatLng.lat;
                        var y = containerLatLng.lng;
                        var inside = false;
                        for (var ii = 0, j = polyPoints.length - 1; ii < polyPoints.length; j = ii++) {
                            var xi = polyPoints[ii].lat, yi = polyPoints[ii].lng;
                            var xj = polyPoints[j].lat, yj = polyPoints[j].lng;
                            var intersect = ((yi > y) != (yj > y))
                                && (x < (xj - xi) * (y - yi) / (yj - yi) + xi);
                            if (intersect) inside = !inside;
                        }
                        if (inside) {
                            selectedFeatures.push(feature);
                        }
                    } else if (feature instanceof L.Polyline) {
                        let closestPoint = feature.closestLayerPoint(containerPoint);
                        if (closestPoint.distance < 3) {
                            selectedFeatures.push(feature);
                        }
                    }
                });
            });

        // for now, just take the first selectedFeature
        let selectedFeature = selectedFeatures.length ? selectedFeatures[0] : null;
        this.setState({ selectedFeature: selectedFeature });
    };


    closeDialog = () => {
        this.setState({ selectedFeature: null });
    };

    saveDialog = () => {

        this.setState({ selectedFeature: null });
    };

	shouldComponentUpdate(nextProps, nextState) {
		let self = this;
		if (!this.initialized && nextProps.map) {
			nextProps.map.on('click', self.handleMapClick, self);
		}
		return true;
	}

	componentWillUnmount() {
		if (this.initialized) {
			this.props.map.off('click', this.handleMapClick, this);
        }
    }

    render() {
        
        return (
            this.state.selectedFeature
            ? <Dialog
                open={this.state.selectedFeature}
                onClose={this.closeDialog}
                aria-labelledby="alert-dialog-title"
                aria-describedby="alert-dialog-description"
            >
                <DialogTitle id="alert-dialog-title">{"Feature Properties"}</DialogTitle>
                <DialogContent>
                    <MaUTable>
                        <TableHead>
                            <TableRow>
                                <TableCell>Property Name</TableCell>
                                <TableCell>Property Value</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>

                                {Object.entries(this.state.selectedFeature.feature.properties).map(([name, value]) => {
                                    return (
                                        <TableRow>
                                            <TableCell>{name}</TableCell>
                                            <TableCell>{value}</TableCell>
                                        </TableRow>
                                    )
                                })}
                        </TableBody>
                    </MaUTable>
                </DialogContent>
                <DialogActions>
                    <Button onClick={this.closeDialog} color="primary" autoFocus>
                        Close
                    </Button>
                    {this.props.layer_begin_edited &&
                        <Button onClick={this.saveDialog} color="primary" >
                            Save
                        </Button>
                    }
                </DialogActions>
            </Dialog>
            : null
		);
    }
};


const mapStateToProps = (state) => {
	return {
		map: state.opengeo.map,
		layers: state.opengeo.layers,
		layer_being_edited: state.opengeo.layer_being_edited
	}
};

export default connect(mapStateToProps)(FeatureAttributesControl);
