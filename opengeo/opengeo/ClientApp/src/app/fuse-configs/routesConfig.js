import React from 'react';
import { Redirect } from 'react-router-dom';
import FuseUtils from '@fuse/utils';

import ForgotPassword2PageConfig from 'app/main/pages/auth/forgot-password-2/ForgotPassword2PageConfig';
import ForgotPasswordPageConfig from 'app/main/pages//auth/forgot-password/ForgotPasswordPageConfig';
import LockPageConfig from 'app/main/pages/auth/lock/LockPageConfig';
import Login2PageConfig from 'app/main/pages/auth/login-2/Login2PageConfig';
import LoginPageConfig from 'app/main/pages/auth/login/LoginPageConfig';
import MailConfirmPageConfig from 'app/main/pages/auth/mail-confirm/MailConfirmPageConfig';
import Register2PageConfig from 'app/main/pages/auth/register-2/Register2PageConfig';
import RegisterPageConfig from 'app/main/pages/auth/register/RegisterPageConfig';
import ResetPassword2PageConfig from 'app/main/pages/auth/reset-password-2/ResetPassword2PageConfig';
import ResetPasswordPageConfig from 'app/main/pages/auth/reset-password/ResetPasswordPageConfig';
import ComingSoonPageConfig from 'app/main/pages/coming-soon/ComingSoonPageConfig';
import Error404PageConfig from 'app/main/pages/errors/404/Error404PageConfig';
import Error500PageConfig from 'app/main/pages/errors/500/Error500PageConfig';
import ProfilePageConfig from 'app/main/pages/profile/ProfilePageConfig';
import MaintenancePageConfig from 'app/main/pages/maintenance/MaintenancePageConfig';

import ExampleConfig from 'app/main/example/ExampleConfig';
import OpenGeoMapPageConfig from 'app/main/opengeo-map/OpenGeoMapPageConfig';

const routeConfigs = [
	ForgotPassword2PageConfig,
	ForgotPasswordPageConfig,
	LockPageConfig,
	Login2PageConfig,
	LoginPageConfig,
	MailConfirmPageConfig,
	Register2PageConfig,
	RegisterPageConfig,
	ResetPassword2PageConfig,
	ResetPasswordPageConfig,
	ComingSoonPageConfig,
	Error404PageConfig,
	Error500PageConfig,
	ProfilePageConfig,
	MaintenancePageConfig,

	ExampleConfig,
	OpenGeoMapPageConfig


];

const routes = [
	...FuseUtils.generateRoutesFromConfigs(routeConfigs),
	{
		path: '/',
		component: () => <Redirect to="/example" />
	}
];

export default routes;
