


import L from 'leaflet';
import axios from 'axios';

L.REST = L.GeoJSON.extend({

    initialize: function (options, geojson) {
        // These come from OL demo: http://openlayers.org/dev/examples/wfs-protocol-transactions.js
        var initOptions = L.extend({
            failure: function (msg) { },    // Function for handling initialization failures
            circleAsMarker: false,        // Treat circles as markers, storing their geometry as a point only. Useful if you've used Leaflet.Draw's pointToLayer to represent points as circles
            setPrimaryKeyOnCreate: true,
            primaryKeyField: 'fid'
            // featureCreatedCallback: function(){} // A function to call after creating a feature
            // featuredEditedCallback: function(){}
            // geomField : <field_name> // The geometry field to use. Auto-detected if only one geom field 
            // url: <WFS service URL> 
            // featureNS: <Feature NameSpace>
            // featureType: <Feature Type>
            // primaryKeyField: <The Primary Key field for using when doing deletes and updates>
            // xsdNs: Namespace used in XSD schemas, XSD returned by TinyOWS uses 'xs:' while GeoServer uses 'xsd:'
            // geoJsonUrl: a URL to use to fetch the geoJson instead of the automatically generated one
        }, options);

        if (typeof initOptions.url == 'undefined') { throw "ERROR: No WFST url declared"; }

        // Call to parent initialize
        L.GeoJSON.prototype.initialize.call(this, geojson, initOptions);

        // Now probably an ajax call to get existing features
        this._loadExistingFeatures();
    },

    // Additional functionality for these functions
    addLayer: function (layer, options) {
        console.log("REST add layer");
        this.restAdd(layer, options);
        // Call to parent addLayer
        L.GeoJSON.prototype.addLayer.call(this, layer);
    },
    removeLayer: function (layer, options) {
        console.log("REST remove layer");
        // do not remove from database at this time
        // defer this until save is pressed and all deleted
        // layers are removed from database at one time
        //this.restRemove(layer, options);

        // Call to parent removeLayer
        L.GeoJSON.prototype.removeLayer.call(this, layer);
    },

    removeLayers: function (layer, options) {
        console.log("REST remove all deleted layer");
        // remove all of these layers from database
        // they have already been removed from map one by one
        this.restRemove(layer, options);
    },

    saveLayers: function (layers, options) {
        console.log("REST save layer");
        this.restSave(layers, options);

        // not adding or removing any layers
    },

    restAdd: function (layers, options) {
        options = options || {};
        layers = layers ? (L.Util.isArray(layers) ? layers : [layers]) : [];

        for (var i = 0, len = layers.length; i < len; i++) {
            this._restAdd(layers[i], options);
        }
    },

    // sends REST request to delete each layer from database
    restRemove: function (layers, options) {
        options = options || {};
        if (layers === null) {
            this._restRemove(null, options);
        }

        layers = layers ? (L.Util.isArray(layers) ? layers : [layers]) : [];

        // original code
        //for (var i = 0, len = layers.length; i < len; i++) {
        //    this._restRemove(layers[i], options);
        //}
        var v;
        for (var i = 0, len = layers.length; i < len; i++) {
            if (typeof layers[i]._layers == 'object') {
                for (v in layers[i]._layers) {
                    this._restRemove(layers[i]._layers[v], options);
                }
            } else {
                this._restRemove(layers[i], options);
            }
        }
    },

    restSave: function (layers, options) {
        options = options || {};
        var realsuccess = options.success;
        layers = layers ? (L.Util.isArray(layers) ? layers : [layers]) : [];

        var v;
        for (var i = 0, len = layers.length; i < len; i++) {
            if (typeof layers[i]._layers == 'object') {
                for (v in layers[i]._layers) {
                    this._restSave(layers[i]._layers[v], options);
                }
            } else {
                this._restSave(layers[i], options);
            }
        }
    },

    _loadExistingFeatures: function () {
        let self = this;
        axios
            .get(this.options.url)
            .then(function (response) {
                let features = response.data.features;
                for (var i = 0, len = features.length; i < len; i++) {
                    features[i]._restSaved = true;
                }
                // capture data
                self.addData(features);
                // capture schema:   geometry type and property names
                self.options.propertyNames = [];
                self.options.geometryType = 'unknown';
                if (features.length > 0) {
                    let feature = features[0];
                    self.options.geometryType = feature.geometry.type;
                    let properties = feature.properties;
                    for (var prop in properties) {
                        self.options.propertyNames.push(prop);
                    }
                }
            })
            .catch(function (error) {
                console.log(error);
                return false;
            });
    },

    _restAdd: function (layer, options) {
        if (typeof layer.feature != 'undefined' &&
            typeof layer.feature._restSaved == 'boolean' &&
            layer.feature._restSaved) {
            return true; // already saved
        }

        // full geoJSON includes type, geometry, and properties (no fid yet, since this is an add)
        // start json off with the type
        let json = '{"type": "Feature", ';  

        // now add the geometry
        json += '"geometry":{"type": "' + this.options.geometryType + '", ';
        if (this.options.geometryType == 'MultiPolygon') {
            json += '"coordinates":[[['; 
            let latlngs = layer._latlngs[0]; // just do one feature
            for (let i = 0; i < latlngs.length; i++) {
                let latlng = latlngs[i];
                json += '[' + latlng.lng.toString() + ',' + latlng.lat.toString() + '],';
            }
            // add first point again to complete polygon
            json += '[' + latlngs[0].lng.toString() + ',' + latlngs[0].lat.toString() + ']';
            json += ']]]}, ';
        }
        if (this.options.geometryType == 'MultiLineString') {
            json += '"coordinates":[[';
            let latlngs = layer._latlngs;
            for (let i = 0; i < latlngs.length; i++) {
                let latlng = latlngs[i];
                json += '[' + latlng.lng.toString() + ',' + latlng.lat.toString() + ']';
                if (i < (latlngs.length - 1)) {
                    json += ',';
                }
            }
            json += ']]}, ';
        }
        if (this.options.geometryType == 'MultiPoint') {
            json += '"coordinates":[';
            json += '[' + layer._latlng.lng.toString() + ', ' + layer._latlng.lat.toString() + ']';
            json += ']}, ';
        }
        if (this.options.geometryType == 'Point') {
            json += '"coordinates":';
            json += '[' + layer._latlng.lng.toString() + ', ' + layer._latlng.lat.toString() + ']';
            json += '}, ';
        }
        // end of geometry
         
        // add empty properties
        json += '"properties": {';
        let propNames = this.options.propertyNames;
        for (let i = 0; i < propNames.length; i++) {
            let propName = propNames[i];
            json += '"' + propName + '": ""';
            if (i < (propNames.length - 1)) {
                json += ', ';
            }
        }
        json += "}"; // end of properties

        json += "}"; // end of feature and this completes the json

        // POST it
        axios
            .post(this.options.url, json, { headers: { 'Content-Type': 'application/json' } })
            .then(function (response) {
                let fid = response.data;
                layer.feature = layer.feature || { properties: {} };
                layer.feature._wfstSaved = true;
                layer.feature.fid = fid;
            })
            .catch(function (error) {
                console.log(error);
                return false;
            });
    },

    _restRemove: function (layer, options) {
        if (typeof this.options.primaryKeyField == 'undefined' && typeof options.where == 'undefined') {
            console.log("I can't do deletes without a primaryKeyField!");
            if (typeof options.failure == 'function') {
                options.failure();
            }
            return false;
        }

        let fid = layer.feature[this.options.primaryKeyField];
        let delete_url = this.options.url + '/' + fid.toString();

        // DELETE it
        axios
            .delete(delete_url)
            .then(function (response) {
                
            })
            .catch(function (error) {
                console.log(error);
                return false;
            });
    },

    //  Save changes to a single layer with WFS-T
    _restSave: function (layer, options) {
        if (typeof this.options.primaryKeyField == 'undefined') {
            console.log("I can't do saves without a primaryKeyField!");
            if (typeof options.failure == 'function') {
                options.failure();
            }
            return false;
        }

        // full geoJSON includes type, id, geometry, and properties
        // start json off with the type
        let json = '{"type": "Feature", ';

        // add feature's unique id - fid
        let fid = layer.feature[this.options.primaryKeyField];
        if (!fid) {
            console.log('No ID for feature.');
            return false;
        }
        json += '"fid": ' + fid.toString() + ', ';

        // now add the geometry
        json += '"geometry":{"type": "' + this.options.geometryType + '", ';
        if (this.options.geometryType == 'MultiPolygon') {
            json += '"coordinates":[[[';
            let latlngs = layer._latlngs[0][0]; // just do one feature
            for (let i = 0; i < latlngs.length; i++) {
                let latlng = latlngs[i];
                json += '[' + latlng.lng.toString() + ',' + latlng.lat.toString() + '],';
            }
            // add first point again to complete polygon
            json += '[' + latlngs[0].lng.toString() + ',' + latlngs[0].lat.toString() + ']';
            json += ']]]}, ';
        }
        if (this.options.geometryType == 'MultiLineString') {
            json += '"coordinates":[[';
            let latlngs = layer._latlngs[0];
            for (let i = 0; i < latlngs.length; i++) {
                let latlng = latlngs[i];
                json += '[' + latlng.lng.toString() + ',' + latlng.lat.toString() + ']';
                if (i < (latlngs.length - 1)) {
                    json += ',';
                }
            }
            json += ']]}, ';
        }
        if (this.options.geometryType == 'MultiPoint') {
            json += '"coordinates":[';
            json += '[' + layer._latlng.lng.toString() + ', ' + layer._latlng.lat.toString() + ']';
            json += ']}, ';
        }
        if (this.options.geometryType == 'Point') {
            json += '"coordinates":';
            json += '[' + layer._latlng.lng.toString() + ', ' + layer._latlng.lat.toString() + ']';
            json += '}, ';
        }
        // end of geometry

        // add empty properties
        json += '"properties": {';
        for (const property in layer.feature.properties) {
            json += '"' + property + '": "' + layer.feature.properties[property].toString() + '", ';
        }
        // remove last comma and space, then end properties and feature
        json = json.substring(0, json.length - 2) + '}}'; 
       
        // PUT it back
        axios
            .put(this.options.url, json, { headers: { 'Content-Type': 'application/json' } })
            .then(function (response) {
                
            })
            .catch(function (error) {
                console.log(error);
                return false;
            });
    }


});