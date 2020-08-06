

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


    $("#microSurveyQuestions").empty();

    formGroupElement = '<div class="form-group">'

    formGroupElement += `<h3 class="nhsuk-heading-s nhsuk-u-margin-bottom-3">${title}</h3>`

    choices.forEach(function (choice) {
      var choiceWithNoSpaces = choice.choiceText.split(" ").join("");
      var choiceNumber = choice.choiceId;
      var inputType = choice.inputType;

      if (choice.showIfServiceType && !choice.showIfServiceType.includes(embeddedData.rec_service_dos_type)) return;

      var textboxDataAttribute = choice.showTextFieldID ? `data-reveals-textbox-id="${choice.showTextFieldID}"` : '';

      var textboxElement = choice.showTextFieldID ? `
            <div class="micro-survey__toggle-element panel nhsuk-u-padding-top-2 nhsuk-u-padding-bottom-2 nhsuk-u-margin-left-3">
              <label for="${choice.showTextFieldID}">
                  ${choice.textFieldLabel}
              </label>
              <input type="text" id="${choice.showTextFieldID}" value="" maxLength="1000">
            </div>
        ` : '';

      formGroupElement += `
          <div class="nhsuk-u-margin-bottom-3">
            <input type="${inputType}" ${textboxDataAttribute} id="${choiceWithNoSpaces}" name="choice" value="${choiceNumber}">
            <label for="${choiceWithNoSpaces}">
                ${choice.choiceText}
            </label>
            ${textboxElement}
          </div>
      `
    })

    formGroupElement += "</div>"

    $("#microSurveyQuestions").append(formGroupElement)

    $("#microSurveyQuestions")
      .append(`<button class="button--next" id="microSurveyNext" type="submit" name="Next" value="Next">Next</button>`);

    if (currentQuestionID != "QID1") {
      $("#microSurveyQuestions").append(`<button class="button--link nhsuk-u-margin-top-4" id="microSurveyPrevious" type="button">Change my previous answer</button>`);
    }

  }

  function displayThanksForYourFeedback() {
    $("#microSurveyQuestions").hide();
    $("#microSurveyEnd").show();
  }

  function displayYouMustSelectOneOption() {
    if ($("#onlySelectOneOption").length === 0) {
      $("#microSurveyQuestions .form-group").addClass("form-group-error")
      $("#microSurveyQuestions h3")
        .after(
          '<span class="field-validation-error error-message nhsuk-u-margin-bottom-4" id="onlySelectOneOption" role="alert">You must select one option</span>');
      $("#microSurveyQuestions input[type='checkbox'], #microSurveyQuestions input[type='radio']" ).attr("aria-invalid", "true")
    }
  }

  function postSurveyAnswers(data) {
    $.ajax({
      url: '/microsurvey/PostRecommendedServiceSurvey',
      method: 'POST',
      contentType: "application/json",
      data: JSON.stringify(data),
      error: function () {
        logEvent(EventTypes.Error, "Micro survey failed")
      }
    });
  };


  function getAnswersForQuestion(questionID) {

    var question = questions[questionID];
    var selectedChoices = $("#microSurveyQuestions input:checked");

    if (selectedChoices.length === 0) return false

    if (question.answerType === "Number") {
      return Number(selectedChoices.val())
    }

    if (question.answerType === "[Number]") {
      var choiceIds = [];

      selectedChoices.each(function () {
        choiceIds.push($(this).val());
        var textFieldID = $(this).attr("data-reveals-textbox-id")
        if (textFieldID) {
          questionsAnswered[textFieldID] = $(`#${textFieldID}`).val()
        }
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

    $("#microSurveyLink").hide()
    $("#microSurveyQuestions").show()
    $(".micro-survey--offering").removeClass("micro-survey--offering")

    questions = _questions
    embeddedData = _embeddedData
    displayQuestion("QID1");

    $('#microSurveyQuestions').on('click', '[data-reveals-textbox-id]', function () {
      var textboxID = $(this).attr('data-reveals-textbox-id')
      $(`#${textboxID}`).show()
    })

    $("#microSurveyQuestions").on('click', '#microSurveyPrevious', function () {
      if (previousQuestionID) displayQuestion(previousQuestionID);
    });

    $("#microSurveyQuestions").on('submit', function () {

      var answers = getAnswersForQuestion(currentQuestionID);
      if (!answers) {
        displayYouMustSelectOneOption();
        return;
      }

      questionsAnswered[currentQuestionID] = answers;

      previousQuestionID = currentQuestionID;

      var nextQuestionID = getNextQuestionID(currentQuestionID, answers)
      if (nextQuestionID) {
        displayQuestion(nextQuestionID)
      }
      else {

        // Displays thank you regardless of success or failure of submitting micro survey
        displayThanksForYourFeedback();

        var questionsAnsweredAndEmbeddedDataMerged = Object.assign(questionsAnswered, embeddedData);

        postSurveyAnswers({
          "values": JSON.stringify(questionsAnsweredAndEmbeddedDataMerged)
        });
      }
    });
  }
});
