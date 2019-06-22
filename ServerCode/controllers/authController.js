db = require('../users/users.json');
var authController = () =>
{

    var verify = (req, res, next) =>
    {
        // get id from header
        let UniqueID = req.headers['x-access-id'];
        console.log("Device: "+UniqueID);
        // error if no id found 
        if(UniqueID == undefined){
            return res.status(404).json({
                success: false,
                message: "No access ID found"
            });
        }
        else
        {
            // fill the request with user data if a user is found
            console.log("User: "+ db.knownLenses[UniqueID])
            if(db.knownLenses[UniqueID] != undefined){
                let user = {};
                user.id =  db.knownUsers[db.knownLenses[UniqueID]];
                user.name = db.knownLenses[UniqueID];
                user.UniqueID = UniqueID;
                req.user = user;
                // go the next function 
                next();
            }
            else
            {
                // if no user is found send a 404
                return res.status(404).json({
                    success: false,
                    message: "No user found"
                });
            }
        }
    }

    return {
        verify
    }
};

module.exports = authController;
