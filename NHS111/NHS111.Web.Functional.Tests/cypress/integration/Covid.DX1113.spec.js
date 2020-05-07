/// <reference types="cypress" />

describe('Covid', () => {
  const staging = 'https://111live:ATeam@staging.111.nhs.uk';
  const url = Cypress.env('Test_Website_url') || staging;

  const startup = () => {
    cy.visit(url);
    cy.get("a[data-event-value='Novel Coronavirus about']").click();
    cy.get('.button--next').click();
    cy.get('#CurrentPostcode').type('LS17 7NZ');
    cy.get('.button--next').click();
    cy.get('.button.button--next').click();

    cy.get('#Female').click();
    cy.get('#UserInfo_Demography_Age').type('22');
    cy.get('.button.button--next').click();

    cy.get('#SymptomsStart_Day').type('6');
    cy.get('#nextScreen').click();

    cy.get('#No').click();
    cy.get('#nextScreen').click();

    cy.get('#Yes').click();
    cy.get('#nextScreen').click();

    cy.get('#No').click();
    cy.get('#nextScreen').click();

    cy.get('input[id="Normal,warmorhot"]').click();
    cy.get('#nextScreen').click();

  };



  it(`- Dx1113 journey 1`, () => {
    startup();

    cy.get('input[id="Yes-I\'vestoppeddoingeverythingIusuallydo"]').click();
    cy.get('#nextScreen').click();
    cy.get('#Id').should('have.value', 'Dx1113');
  });

  it(`- Dx1113 journey 2`, () => {
    startup();

    cy.get('input[id="No-Ifeelwellenoughtodomostofmyusualdailyactivities"]').click();
    cy.get('#nextScreen').click();

    cy.get(`input[id="Yes"]`).click();
    cy.get('#nextScreen').click();

    cy.get('#Id').should('have.value', 'Dx1113');
  });


});
