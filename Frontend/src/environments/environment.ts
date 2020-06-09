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
  showStackTrace: true,

  apiUrl: 'backendURL/api/',
  apiUrlNoSSL: 'backendURL/api/',

  notificationHubUrl: 'backendURL/api/notifications-hub',
  notificationHubUrlNoSSL: 'backendURL/api/notifications-hub',

  // https://numverify.com/dashboard
  // phone validation
  numverifyAccess: 'YourAccessKey'
};
