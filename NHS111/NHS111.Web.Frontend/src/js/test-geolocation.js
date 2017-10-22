import {expect, assert} from 'chai'
import geolocation from './geolocation.js'
import {JSDOM} from 'jsdom'
import {jQuery} from 'jquery'

const mockGeolocation = function (callback) {
    return callback({ coords: { latitude: 50.9329175, longitude: -1.3093775 } })
}

const mockNavigator = { geolocation: { getCurrentPosition: mockGeolocation } }

describe('Validation - geolocation', _ => {

    it('should fail if geolocation is not available', () => {
        return geolocation.getCoordinates().catch((err) => {
            assert.exists(err)
        })
    })

    it('should return coordinates', () => {
        const DOM = new JSDOM(`<!DOCTYPE html>`)
        return geolocation.getCoordinates(mockNavigator).then((pos) => {
            assert.isNotNull(pos)
        })
    })

    /*
    it('should return correct addresses', () => {
        const DOM = new JSDOM(`<!DOCTYPE html>`)
        const $ = require('jquery')(DOM.window)
        return geolocation.getAddressLookup({ latitude: 50.9329175, longitude: -1.3093775 }, DOM).then((addresses) => {
            console.log(addresses)
            assert.isNotNull(addresses)
        })
    })*/



})
