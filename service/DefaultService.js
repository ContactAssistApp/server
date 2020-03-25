'use strict';


/**
 * Post location history
 *
 * body Body
 * id String Id of this location history
 * no response value expected for this operation
 **/
exports.dataIdPOST = function(body,id) {
  return new Promise(function(resolve, reject) {
    console.log(body)
    console.log(id)
    resolve();
  });
}
