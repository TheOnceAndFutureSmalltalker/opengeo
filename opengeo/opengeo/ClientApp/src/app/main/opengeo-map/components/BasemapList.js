import React, { Component } from 'react';
import ListItemText from "@material-ui/core/ListItemText";
import List from "@material-ui/core/List";
import Paper from "@material-ui/core/Paper";
import ListItem from "@material-ui/core/ListItem";
import ListItemIcon from "@material-ui/core/ListItemIcon";
import Radio from "@material-ui/core/Radio"
import L from 'leaflet';
import { connect } from 'react-redux';
import { selectedBasemap } from '../actions'



class BasemapList extends Component {
    constructor(props) {
        super(props);
        this.initialized = false;
        this.leaflet_layer = null;
    }

    removeBasemap(basemap) {
        if (this.leaflet_layer) {
            this.props.map.removeLayer(this.leaflet_layer);
            this.leaflet_layer = null;
        }
    }

    addBasemap(basemap) {
        this.leaflet_layer = L.tileLayer.wms(basemap.url, { layers: '0' });
        this.props.map.addLayer(this.leaflet_layer);
    }

    toggleBasemap = (e, newBasemap) => {  // basemapInput is []
        this.props.basemap.checked = false;
        this.removeBasemap(this.props.basemap);

        newBasemap.checked = true;
        this.addBasemap(newBasemap);
        this.props.selectedBasemap( newBasemap );
    };

    shouldComponentUpdate(nextProps, nextState) {
        if (!this.initialized && nextProps.basemap && nextProps.map && nextProps.basemaps) {
            nextProps.basemaps.forEach(bm => {
                if (bm.id === nextProps.basemap.id) {
                    bm.checked = true;
                    this.addBasemap(bm);
                    // initially, props.basemap is not from the basemaps list but set from the map data
                    // so find the corresponding basemap in basemaps list and set that to the props.basemap
                    this.props.selectedBasemap(bm);
                } else {
                    bm.checked = false;
                }
            });
            this.initialized = true;
            return false;
        }
        return true;
    }

    render() {
        return (
            (this.props.basemaps && this.props.map && this.props.basemap)
                ? <div>
                    <h2 style={{ padding: 5, paddingLeft: 10 }}>Basemap</h2>
                    <Paper style={{ maxHeight: this.props.height, overflow: 'auto' }}>
                        
                        <List >
                            {this.props.basemaps.map((bm) => {
                                return (
                                    <ListItem key={bm.name} >
                                        <ListItemIcon>
                                            <Radio
                                                onClick={(e) => this.toggleBasemap(e, bm)}
                                                edge="start"
                                                checked={bm.checked}
                                                size='small'
                                            />
                                        </ListItemIcon>
                                        <ListItemText primary={bm.name} />
                                    </ListItem>
                                );
                            })}
                        </List>
                     </Paper>
                </div>
                : null
        );
    }
}

const mapStateToProps = (state) => {
    return {
        map: state.opengeo.map,
        basemap: state.opengeo.basemap,
        basemaps: state.opengeo.basemaps
    }
};

export default connect(mapStateToProps, { selectedBasemap })(BasemapList);

