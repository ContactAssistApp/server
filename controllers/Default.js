'use strict';

var utils = require('../utils/writer.js');
var Default = require('../service/DefaultService');

module.exports.dataIdPOST = function dataIdPOST (req, res, next, body, id) {
  Default.dataIdPOST(body, id)
    .then(function (response) {
      utils.writeJson(res, response);
    })
    .catch(function (response) {
      utils.writeJson(res, response);
    });
};
