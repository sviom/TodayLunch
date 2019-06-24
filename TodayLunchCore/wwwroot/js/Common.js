function Test(type, url, datatype, contentType, data) {
//function Test() {
    return new Promise(function (resolve, reject) {
        var result = ExecuteAjax(type, url, datatype, contentType, data);        
        //resolve(result);
        if (result === null) {
            reject("error occured");
        } else {
            resolve("Test Message");
        }
    });
}

/// AJAX 실행(타입/주소/데이터타입/콘텐츠타입/데이터)
function ExecuteAjax(type, url, datatype, contentType, data) {
    var returnData;
    if (data === null || data === undefined) {
        $.ajax({
            type: type,
            url: url,
            dataType: datatype,
            contentType: contentType,
            //async: true,
            error: function (errorData) {
                console.log("통신실패!");
                returnData = errorData;
            },
            success: function (successData) {
                console.log("통신데이터 값 : ");
                console.log(successData);
                returnData = successData;
            }
        });
    }
    else {
        $.ajax({
            type: type,
            url: url,
            dataType: datatype,
            contentType: contentType,//"application/json",
            data: data,
            //async: true,
            error: function (errorData) {
                console.log("통신실패!");
                returnData = errorData;
            },
            success: function (successData) {
                console.log("통신데이터 값 : ");
                console.log(successData);
                returnData = successData;
            }
        });
    }

    return returnData;
}