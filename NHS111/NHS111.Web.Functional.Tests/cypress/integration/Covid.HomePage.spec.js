/// <reference types="cypress" />

describe('Covid homepage', () => {
    const staging = 'https://111live:ATeam@staging.111.nhs.uk';
    const develop = 'http://localhost:59010';
    const url = Cypress.env('Test_Website_url') || staging;

    const startup = () => {
        cy.visit(`${url}/service/covid-19`);
    };

    it(`- start symptom checker from "Start now"`, () => {
        startup();

        cy.get('#start-symptom-checker').click();
        cy.get('#CurrentPostcode').should('exist');
    });

    it(`- start symptom checker from link`, () => {
        startup();

        cy.get('#start-symptom-checker-link').click();
        cy.get('#CurrentPostcode').should('exist');
    });

    it(`- can go to NHS UK website`, () => {
        startup();

        cy.get('a[href*="nhs.uk/conditions/coronavirus-covid-19"]').should('exist');
    });

    it(`- can start isolation note flow`, () => {
        startup();

        cy.get('a[data-event-value="Get an isolation note"').should('exist');
    });

});
