const fs = require('fs');
const fileHelper = rootRequire("helpers/FileHelper");

// TODO: make all read file functions and write file functions async and move them to a helper.
let saveFileController = () => {

    // function to get save data out of a file and send it back
    let getOwnSaveFile = (req, res) => {
        let data;
        // get parameters if they are there 
        let mode = req.params.mode;
        console.log(mode);
        // create path based on mode
        let filePath;
        if (mode == undefined) {
            filePath = 'saveData/data_' + req.user.name + '.json';
        } else {
            filePath = 'saveData/data_' + req.user.name + '_' + mode + '.json';
        }
        // read file found at path
        fs.readFile(filePath, 'utf-8', function (error, data) {
            // no error? nice here is your data 
            if (error == null) {
                return res.status(200).json({
                    success: true,
                    SaveDataWrapper: JSON.parse(data)
                });
            }
            else {
                // check if user requested a mode if so there needs to be a "base" savedata file because we don't want to create a new one from scratch
                if (mode != undefined) {
                    // check if default is avalible and copy it over
                    // newSave = fileHelper.readFile('saveData/data_' + req.user.name + '.json');
                    fs.readFile('saveData/data_' + req.user.name + '.json', 'utf-8', function (error, data) {
                        // no error? greate we can use this data and write the file
                        if (error == null) {
                            console.log(JSON.parse(data));
                            // write file
                            fs.writeFile(filePath, JSON.stringify(JSON.parse(data)) , function (error) {
                                if (error == null) {
                                    // if all went well send back the data 
                                    return res.status(200).json({
                                        success: true,
                                        SaveDataWrapper: JSON.parse(data)
                                    });
                                }
                                // error give it back to the user
                                else {
                                    return res.status(402).json({
                                        success: false,
                                        message: "writing new file failed"
                                    });
                                }
                            });
                        } else {
                            // we are not going to write a "child" file if there is no parent reference 
                            return res.status(402).json({
                                success: false,
                                message: "Can't create mode without reference SaveData"
                            });
                        }
                    });
                } else {
                    // if no mode is used just create a fresh new savefile 
                    fs.writeFile(filePath, JSON.stringify({ "anchorData": [], "saveData": [] }), function (error) {
                        if (error == null) {
                            return res.status(200).json({
                                success: true,
                                SaveDataWrapper: JSON.stringify({ "anchorData": [], "saveData": [] })
                            });
                        }
                        else {
                            return res.status(402).json({
                                success: false,
                                message: "writing new file failed"
                            });
                        }
                    });
                }
            }
        });
    };
    // function to save a file 
    let setSaveFile = (req, res) => {
        // get all the data from the body
        let data = req.body;
        console.log(data);
        // get the parameters from the url
        let mode = req.params.mode;
        console.log(mode);
        // create the desired path based on the parameters
        let filePath;
        if (mode == undefined) {
            filePath = 'saveData/data_' + req.user.name;
        } else {
            filePath = 'saveData/data_' + req.user.name + '_' + mode;
        }
        // check if the json is in the correct format
        if (!data.hasOwnProperty("anchorData") || !data.hasOwnProperty("saveData")) {
            return res.status(402).json({
                success: false,
                message: "missing data"
            });
        }
        // rename the old file as a backup 
        fs.rename(filePath+ '.json', filePath +' '+ new Date() + '.json', function (error) {
            if (error) {
                // if the backup errored stop request 
                return res.status(402).json({
                    success: false,
                    message: "could not make backup of old file"
                });
            } else {
                // if the backup was succesfull create the new save file
                fs.writeFile(filePath+ '.json', JSON.stringify(data), function (error) {
                    if (error == null) {
                        return res.status(200).json({
                            success: true,
                            // message: "new file created",
                            data: data
                        });
                    }
                });
            }
        });
    };

    return {
        getOwnSaveFile,
        setSaveFile
    };
};

module.exports = saveFileController;
