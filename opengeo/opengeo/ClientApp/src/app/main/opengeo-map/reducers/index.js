
import { combineReducers } from 'redux';
import * as Actions from '../actions';

// controls static list of basemaps available to the map/project
const basemapsReducer = (basemaps=[], action) => {
    if (action.type === Actions.BASEMAPS_ACCESSED) {
        return action.value;
    }
    return basemaps;
};

// sets the current leaflet map
const mapReducer = (map=null, action) => {
    if (action.type === Actions.MAIN_MAP_CREATED) {
        return action.value;
    }
    return map;
};

// controls map_parameters id, zoom, lat, lng
// these are changed when read from API and when user pans/zooms map
const mapParametersReducer = (map_parameters = null, action) => {
    if (action.type === Actions.MAP_PARAMETERS_ACCESSED) {
        return action.value;
    }
    if (action.type === Actions.MAP_PARAMETERS_CHANGED) {
        return action.value;
    }
    return map_parameters;
};

// controls list of layers in map/project
// layers are not added or edited, just their features are edited
const layersReducer = (layers = null, action) => {
    if (action.type === Actions.LAYERS_ACCESSED) {
        return action.value;
    }
    if (action.type === Actions.LAYERS_UPDATED) {
        return action.value;
    }
    return layers;
}

// controls the selected basemap for the map/project
// initially from API then selected by user
const basemapReducer = (basemap = null, action) => {
    if (action.type === Actions.BASEMAP_ACCESSED) {
        return action.value;
    }
    if (action.type === Actions.BASEMAP_SELECTED) {
        return action.value;
    }
    return basemap;
}

// controls the layer selected for editing
const layerBeginEditedReducer = (layer = null, action) => {
    if (action.type == Actions.LAYER_BEING_EDITED_SELECTED) {
        return action.value;
    }
    return layer;
};


const opengeoReducers = combineReducers({
    map: mapReducer,
    basemaps: basemapsReducer,
    map_parameters: mapParametersReducer,
    layers: layersReducer,
    basemap: basemapReducer,
    layer_being_edited: layerBeginEditedReducer
});

export default opengeoReducers;