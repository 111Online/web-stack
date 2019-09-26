import {expect, assert} from 'chai'
import validation from './validation.js'
import {JSDOM} from 'jsdom'
import {jQuery} from 'jquery'

describe('Validation - number', _ => {
  it ('should return an integer', () => {
    let result = validation.number("3")
    assert.isNumber(result)
  })

  it ('should ensure a number entered does not get changed', () => {
    let result = validation.number("4534353")
    expect(result).to.equal(4534353)
  })

  it ('should ensure any non-numerical characters are stripped', () => {
    // Test as a string to make sure the returned number is exactly as expected
    let result = (validation.number("=4534e3-5s3")).toString()
    expect(result).to.equal("4534353")
  })

  it ('should allow negative values', () => {
    let result = validation.number("-4534353")
    expect(result).to.equal(-4534353)
  })
})


describe('DOM Validation: .js-validate-number', _ => {

  it ('should convert invalid input to valid', () => {
    const DOM = new JSDOM(`<!DOCTYPE html><input type="number" class="js-validate-number">`)
    const $ = require('jquery')(DOM.window)

    $('.js-validate-number').on('keypress keydown input', function (e) {
      var text = $(this).val()
      var min =  $(this).attr('min')
      var max =  $(this).attr('max')
      var number = validation.number(text, min, max)
      $(this).val(number)
    });

    $('.js-validate-number').val('=4534e3-5s3').trigger("input")

    expect($("input").val()).equal('4534353')

  })
})


describe('Validation - date', _ => {
  it ('should return the date given as a Date object', () => {
    let result = validation.date(2017, 6, 1)
    expect(result.toDateString()).equal("Thu Jun 01 2017")
  })

  it ('should return undefined if a date is not valid', () => {
    let result = validation.date(2017, 36, 134)
    expect(result).to.be.undefined
  })
})
