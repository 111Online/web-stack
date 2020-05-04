/// <reference types="cypress" />

describe('Covid', () => {
  const staging = 'https://111live:ATeam@staging.111.nhs.uk';
  const url = Cypress.env('Test_Website_url') || staging;

  const scenarios = [
    {
      age: '22',
      gender: 'Female',
      dispositionCodeNotCovid: 'PX113.3.PW1851.3',
      dispositionCode: 'PX115.5.PW1851.5',
      nextQuestionId: 'PW556.6800'
    },
    {
      age: '22',
      gender: 'Male',
      dispositionCodeNotCovid: 'PX113.3.PW1851.3',
      dispositionCode: 'PX115.5.PW1851.5',
      nextQuestionId: 'PW559.5200'
    },
    {
      age: '11',
      gender: 'Female',
      dispositionCodeNotCovid: 'PX117.1.PW1852.1',
      dispositionCode: 'PX118.0.PW1852.0',
      nextQuestionId: 'PW557.6600'
    },
    {
      age: '11',
      gender: 'Male',
      dispositionCodeNotCovid: 'PX117.1.PW1852.1',
      dispositionCode: 'PX118.0.PW1852.0',
      nextQuestionId: 'PW560.5000'
    }
  ];

  const startup = ({
    age, gender
  }) => {
    cy.visit(url);
    cy.get('.button--next').click();
    cy.get('#CurrentPostcode').type('LS17 7NZ');
    cy.get('.button--next').click();
    cy.get('.button.button--next').click();

    cy.get(`#${gender}`).click();
    cy.get('#UserInfo_Demography_Age').type(age);
    cy.get('.button.button--next').click();

    cy.get('#covid19-search-link').click();

    cy.get('#SymptomsStart_Day').type('6');
    cy.get('#nextScreen').click();
  };

  const notCovidJourney = ({
    age, gender, dispositionCodeNotCovid
  }) => {
    startup({ age, gender });

    cy.get('#No').click();
    cy.get('#nextScreen').click();

    cy.get('#No').click();
    cy.get('#nextScreen').click();

    cy.get('#No').click();
    cy.get('#nextScreen').click();

    cy.get('#Id').should('have.value', dispositionCodeNotCovid);
  };

  const moreQuestions = ({
    age, gender, dispositionCode, nextQuestionId
  }) => {
    startup({ age, gender });

    cy.get('#No').click();
    cy.get('#nextScreen').click();

    cy.get('#No').click();
    cy.get('#nextScreen').click();

    cy.get('#Yes').click();
    cy.get('#nextScreen').click();

    cy.get('#Id').should('have.value', dispositionCode);
    cy.get('#nextScreen').click();

    cy.get('#Id').should('have.value', nextQuestionId);
  };

  scenarios.forEach(({ age, gender, dispositionCodeNotCovid, dispositionCode, nextQuestionId }) => {
    it(`- Unlikely to have Covid disposition, ${gender}, ${age}`, () => {
      notCovidJourney({
        age, gender, dispositionCodeNotCovid
      });
    });

    it(`- More questions to Covid disposition, ${gender}, ${age}`, () => {
      moreQuestions({
        age, gender, dispositionCode, nextQuestionId
      });
    });
  });
});
