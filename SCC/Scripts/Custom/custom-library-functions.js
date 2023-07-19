async function fetchDatatableBasicData(path = '../Content/files/datatables-basic-data.json') {
    const response = await fetch(path);
    const data = await response.json();
    return data;
}

async function fetchMultiSelectBasicData(path = '../Content/files/multi-select-basic-data.json') {
    const response = await fetch(path);
    const data = await response.json();
    return data;
}

function startMultiSelect(selector, data) {
    $(selector).chosen(data);
}

function startDataTable(selector, data) {
    let currentTable = $(selector).DataTable(data);
}

function loadHtmlToControl(url, controlID, successFunction, errorFunction) {
    fetch(url).then(function (response) {
        return response.text();
    }).then(function (html) {
        $('#' + controlID).html(html);
        if (successFunction != null)
            successFunction(html);
    }).catch(function (err) {
        console.warn('Something went wrong.', err);

        if (errorFunction != null)
            errorFunction(err);
    });
}

function tableSearchBoxValue(tableSelector, value) {
    if (value != null) {
        let datatable = $(tableSelector).DataTable();
        datatable.search(value, false, false).draw();
    }
    else {
        return $(tableSelector).DataTable().search();
    }
}