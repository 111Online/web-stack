/// <reference types="cypress" />

describe('Covid', () => {
  const url = Cypress.env('Test_Website_url');

  const navigateToDispositionDx391 = () => {
    cy.visit(url);
    cy.get("a[data-event-value='Novel Coronavirus about']").click();
    cy.get('#start-symptom-checker').click();
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

    cy.get('#No-Ifeelwellenoughtodomostofmyusualdailyactivities').click();
    cy.get('#nextScreen').click();

    cy.get(`input[id="I'mnotsure"]`).click();
    cy.get('#nextScreen').click();

    cy.get(`input[id="I'mnotsure"]`).click();
    cy.get('#nextScreen').click();

    cy.get(`input[id="I'mnotsure"]`).click();
    cy.get('#nextScreen').click();

  };

  it(`- DX391`, () => {
    navigateToDispositionDx391();
    cy.get('#Id').should('have.value', 'Dx391');

  });

  it(`- Dx391 to stay at home`, () => {
    navigateToDispositionDx391();
    cy.get('a[data-event-value="gov.uk coronavirus testing page"]').should('exist');

  });
});
