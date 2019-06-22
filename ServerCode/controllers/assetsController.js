var fs = require('fs');
var assetController = () =>
{
    // function that gives back the assethelper data, that way we have a fast and easy way to know what assets belong to what user
    let getAssetListOfType = (req, res) => {
        // check if user directory exists
        // make if not
        fs.mkdir('./assetHelpers/'+req.user.name+'/' ,function(e){
            if(!e || (e && e.code === 'EEXIST')){
                // check if file of type exists
                fs.readFile('./assetHelpers/'+req.user.name+'/'+req.params.type + '.json', 'utf-8', function (error,data) {
                    if (error == null) {
                        return res.status(200).json({
                            success: true,
                            amount: JSON.parse(data).length,
                            assets: JSON.parse(data)
                        });
                    }else{
                         // if not send 0 assets back
                         return res.status(200).json({
                            success: true,
                            amount: 0,
                            assets:[]
                        });
                    }
                });

            } else {
                return res.status(500).json({
                    success: false,
                    message: 'I dont know what happend but something went wrong when looking for a directory!',
                    error: e
                });
            }
        });
    };

    // this process needs to be handled in the app.js before the router or the data gets lost :(
    // let addAsset = (req, res) => {
    //     // check if directory exists
    //     // make if not
    //     let assets = [];
    //     let newAssets = req.body;
    //     fs.mkdir('./assetHelpers/'+req.user.name+'/' ,function(e){
    //         if(!e || (e && e.code === 'EEXIST')){
    //             // check if file of type exists
    //             fs.readFile('./assetHelpers/'+req.user.name+'/'+req.params.type + '.json', 'utf-8', function (error, data) {
    //                 if (error == null) {
    //                     if(data != undefined){
    //                         assets = JSON.parse(data);
    //                     }
    //                 }
    //                 for(let i= 0;i > newAssets.length; i++ ){
    //                     console.log("test")
    //                     if(assets.includes(newAssets[i])){
                            
    //                         newAssets.splice(i);
    //                     }
    //                 }
    //                 assets = assets.concat(newAssets.assets)
    //                 fs.writeFile('./assetHelpers/'+req.user.name+'/'+req.params.type + '.json', JSON.stringify(assets,true,true), function (error) {
    //                     if(error == null){
    //                         return res.status(200).json({
    //                             success: true,
    //                             assets: assets
    //                         });
    //                     }
    //                 });
    //             });
    //         } else {
    //             return res.status(500).json({
    //                 success: false,
    //                 message: 'I dont know what happend but something went wrong when looking for a directory!',
    //                 error: e
    //             });
    //         }
    //     }); 
    // }

    return {
        getAssetListOfType,
        // addAsset
    };
};
module.exports = assetController;
