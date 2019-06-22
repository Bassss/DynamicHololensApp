const fs = require('fs');

async function readFile(path) {
    const data = null;
    await fs.readFile(path, 'utf-8', function (error, data) {
        if (error == null) {
           data = JSON.parse(data);
           console.log(data);
        }
    });
    console.log(data);
    return data
}
module.exports = {
    readFile
};