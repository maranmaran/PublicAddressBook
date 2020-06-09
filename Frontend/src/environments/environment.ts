/**
 * TEMPLATE FOR ENVIRONMENTS
 * Currently angular.json is set up for
 * dev
 * prod
 * Environments
 * Please create environment.dev and envioronment.prod and fill this template inside
 */
export const environment = {

  name: 'environmentName', // depends on your env
  production: false, // depends on your env
  showStackTrace: true,  // depends on your env

  apiUrl: 'backendURL/api/',
  apiUrlNoSSL: 'backendURL/api/',

  notificationHubUrl: 'backendURL/api/notifications-hub',
  notificationHubUrlNoSSL: 'backendURL/api/notifications-hub',

  // https://numverify.com/
  phoneValidationServiceAPIKey: 'API_KEY',

  // google maps geocoding service
  // -- not free :) 

  // https://www.neutrinoapi.com/
  // failed to get it to work for dev server -- CORS issue on API

  // addressValidationServiceUserID: 'maranmaran',
  // addressValidationServiceAPIKey: '3Mt9wR8abVYFHlmsShIwgb0MHacx4BU85QZQ6jLvNxI65w4U'
};
