/**
 * 根据用户名获取用户信息
 * @param {any} userName
 */
function GetUserId(userName) {
            /// change this prefix according to the environment. 
            /// In below sample, windows authentication is considered.
            var prefix = "i:0#.w|";
            /// get the site url
            var siteUrl = “http://akmiidev/sites/workflowcenter”;
            /// add prefix, this needs to be changed based on scenario
            var accountName = prefix + userName;

            /// make an ajax call to get the site user
            $.ajax({
                url: siteUrl + "/_api/web/siteusers(@v)?@v='" + 
                    encodeURIComponent(accountName) + "'",
                method: "GET",
                headers: { "Accept": "application/json; odata=verbose" },
                success: function (data) {
                    ///popup user id received from site users.
                    alert("Received UserId" + data.d.Id);
                    alert(JSON.stringify(data));
                },
                error: function (data) {
                    console.log(JSON.stringify(data));
                }
            });
        }