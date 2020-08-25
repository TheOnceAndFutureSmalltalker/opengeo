import auth from 'app/auth/store/reducers';
import { combineReducers } from 'redux';
import fuse from './fuse';
import opengeo from 'app/main/opengeo-map/reducers';

const createReducer = asyncReducers =>
	combineReducers({
		auth,
		fuse,
		opengeo,
		...asyncReducers
	});

export default createReducer;
