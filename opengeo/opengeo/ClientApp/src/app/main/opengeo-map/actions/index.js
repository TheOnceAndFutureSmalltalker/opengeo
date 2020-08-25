export const MAIN_MAP_CREATED = 'MAIN_MAP_CREATED';
export const BASEMAPS_ACCESSED = 'BASEMAPS_ACCESSED';
export const MAP_PARAMETERS_ACCESSED = 'MAP_PARAMETERS_ACCESSED';
export const MAP_PARAMETERS_CHANGED = 'MAP_PARAMETERS_CHANGED';
export const BASEMAP_ACCESSED = 'BASEMAP_ACCESSED';
export const BASEMAP_SELECTED = 'BASEMAP_SELECTED';
export const LAYERS_ACCESSED = 'LAYERS_ACCESSED';
export const LAYERS_UPDATED = 'LAYERS_UPDATED';
export const LAYER_BEING_EDITED_SELECTED = 'LAYER_BEING_EDITED_SELECTED';


// the main leaflet map has been created
export const createMainMap = (map) => {
    return {
        type: MAIN_MAP_CREATED,
        value: map
    };
};


// list of available basemaps accessed from API
export const accessedBasemapList = (basemaps) => {
    return {
        type: BASEMAPS_ACCESSED,
        value: basemaps
    };
};


// object of zoom level, lat, lng accessed from API
export const accessedMapParameters = (map_parameters) => {
    return {
        type: MAP_PARAMETERS_ACCESSED,
        value: map_parameters
    };
};


// user/application changed zoom, lat, lng of map
export const changedMapParameters = (map_parameters) => {
    return {
        type: MAP_PARAMETERS_CHANGED,
        value: map_parameters
    };
};


// accessed basemap of a project using API
export const accessedBasemap = (basemap) => {
    return {
        type: BASEMAP_ACCESSED,
        value: basemap
    };
};


// user/application selected a different basemap of project
export const selectedBasemap = (basemap) => {
    return {
        type: BASEMAP_SELECTED,
        value: basemap
    };
};


// accessed layers of a map/project using API
export const accessedLayers = (layers) => {
    return {
        type: LAYERS_ACCESSED,
        value: layers
    };
};


// user/application edited a layer (added/edited/removed a feature)
export const updatedLayers = (layers) => {
    return {
        type: LAYERS_UPDATED,
        value: layers
    };
};


// user selected/unselected layer for editing
export const selectedLayerForEditing = (layer) => {
    return {
        type: LAYER_BEING_EDITED_SELECTED,
        value: layer
    };
};

