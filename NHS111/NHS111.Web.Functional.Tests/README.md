# Web Functional tests

## how to run them

### Selenium Webdriver tests

#### From Visual Studio

1. right click on Tests/Functional/NHS111.Web.Functional.Tests
2. select run tests

### Cypress (https://www.cypress.io/)

#### prerequisite

Node is installed.

#### download/install node packages

Install node packages: `npm i`

#### Run/debug

Run cypress tets: `npm test`

Debug cypress tests: `npm run open` will open cypress test runner (this is a standalone app, see https://www.cypress.io)

#### Test Files

Test files can be found in cypress/integration folder