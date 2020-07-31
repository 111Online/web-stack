

jQuery(document).ready(function () {
  var questions = {}
  var embeddedData = {}
  var currentQuestionID = "QID1";
  var previousQuestionID = null;

  var questionsAnswered = {};

  function displayQuestion(questionID) {
    currentQuestionID = questionID;

    var title = questions[questionID].title;
    var choices = questions[questionID].choices;


    $("#recommendedSurveyFeedback").empty();

    $("#recommendedSurveyFeedback").append('<p id="questionTitle"><strong>' + title + '</strong></p>');

    choices.forEach(function (choice) {
      var choiceWithNoSpaces = choice.choiceText.split(" ").join("");
      var choiceNumber = choice.choiceId;
      var inputType = choice.inputType;

      if (choice.showIfServiceType && !choice.showIfServiceType.includes(embeddedData.serviceType)) return;

      $("#recommendedSurveyFeedback")
        .append(`
                        <input type="${inputType}" id="${choiceWithNoSpaces}" name="choice" value="${choiceNumber}">
                        <label id="${choiceWithNoSpaces}Label" for="${choiceWithNoSpaces}">
                            ${choice.choiceText}
                        </label>
                        <br>\r\n
                    `);
    });

    $("#recommendedSurveyFeedback")
      .append(`<button class="button--next" id="microSurveyNext" type="button" name="Next" value="Next">Next</button>\r\n`);
  }

  function displayThanksForYourFeedback() {

    var thankYouContent = `
                <p><strong>Thanks for your feedback</strong></p>
                <p>We\'ll use it to improve the services we recommend.</p>
                <p>You can help improve the whole 111 online service by <a href="">taking our survey (opens in a new tab or window)</a></p>
            `;

    $("#recommendedSurveyFeedback").html(thankYouContent)
  }

  function displayYouMustSelectOneOption() {
    if ($("#onlySelectOneOption").length === 0) {
      $("#questionTitle")
        .after(
          '<p class="field-validation-error error-message" id="onlySelectOneOption">You must select one option</p>');
    }
  }

  function postSurveyAnswers(data) {
    $.ajax({
      url: '/microsurvey/PostRecommendedServiceSurvey',
      method: 'POST',
      contentType: "application/json",
      data: JSON.stringify(data),
      success: function () {
        alert('Success!');
      },
      error: function (resultData) {
        alert('Failure!');
      }
    });
  };


  function getAnswersForQuestion(questionID) {

    var question = questions[questionID];

    if (question.answerType === "Number") {
      return $("#recommendedSurveyFeedback input:checked").attr("value")
    }

    if (question.answerType === "[Number]") {
      var selectedChoices = $('#recommendedSurveyFeedback input[type=checkbox]:checked');

      if (selectedChoices.length === 0) {
        displayYouMustSelectOneOption();
      }

      var choiceIds = [];

      selectedChoices.each(function () {
        choiceIds.push($(this).attr('value'));
      });

      return choiceIds;
    }
  }

  function getNextQuestionID(questionID, answers) {
    var answersArray = answers instanceof Array ? answers : [answers];
    var question = questions[questionID];

    var nextQuestionID;
    answersArray.forEach((answer) => {
      var filtered = question.choices.filter((choice) => choice.choiceId === Number(answer))
      if (filtered.length && filtered[0].nextQuestionID) nextQuestionID = filtered[0].nextQuestionID
    })

    return nextQuestionID
  }

  // Listeners
  window.startMicroSurvey = function (_questions, _embeddedData) {
    questions = _questions
    embeddedData = _embeddedData
    displayQuestion("QID1");

    $("#recommendedSurveyFeedback").on('click', '#changeMyPreviousAnswer', function () {
      if (previousQuestionID) displayQuestion(previousQuestionID);
    });

    $("#recommendedSurveyFeedback").on('click', '#microSurveyNext', function () {

      var answers = getAnswersForQuestion(currentQuestionID);
      questionsAnswered[currentQuestionID] = answers;

      previousQuestionID = currentQuestionID;

      var nextQuestionID = getNextQuestionID(currentQuestionID, answers)
      if (nextQuestionID) {
        displayQuestion(nextQuestionID)
      }
      else {
        displayThanksForYourFeedback();

        postSurveyAnswers({
          "values": JSON.stringify(questionsAnswered)
        });
      }
    });

  }

});
