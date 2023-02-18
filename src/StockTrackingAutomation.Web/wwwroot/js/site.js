// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

SelectNav();
InitDataTable();
function SelectNav() {
    var currentUrl = window.location.href;
    var navBarItems = document.getElementsByClassName("nav-link");
    if (currentUrl.includes("Home") && currentUrl.includes("Statistics")) {
        var myElement = navBarItems[0];
        myElement.classList.add("active");
        return;
    }
    if (currentUrl.includes("Product")) {
        var myElement = navBarItems[1];
        myElement.classList.add("active");
        return;
    }
    if (currentUrl.includes("Purchase")) {
        var myElement = navBarItems[2];
        myElement.classList.add("active");
        return;
    }
    if (currentUrl.includes("Sale")) {
        var myElement = navBarItems[3];
        myElement.classList.add("active");
        return;
    }
    if (currentUrl.includes("DebtLog")) {
        var myElement = navBarItems[4];
        myElement.classList.add("active");
        return;
    }
    if (currentUrl.includes("Customer")) {
        var myElement = navBarItems[5];
        myElement.classList.add("active");
        return;
    }
    if (currentUrl.includes("Supplier")) {
        var myElement = navBarItems[6];
        myElement.classList.add("active");
        return;
    }
    if (currentUrl.includes("User")) {
        var myElement = navBarItems[7];
        myElement.classList.add("active");
        return;
    }
    if (currentUrl.includes("ChangePassword")) {
        var myElement = navBarItems[9];
        myElement.classList.add("active");
        return;
    }
    if (currentUrl.includes("Account")) {
        var myElement = navBarItems[8];
        myElement.classList.add("active");
        return;
    }
}

function RemoveActiveFromAllNav() {
    var navBarItems = document.getElementsByClassName("nav-link");
    for (var i = 0; i < navBarItems.length; i++) {
        var item = navBarItems[i];
        item.classList.remove("active");
    }
}

function InitDataTable()
{
    var tableAny = document.getElementById("my_DataTable");
    if (tableAny) {
        $(document).ready(function () {
            $('#my_DataTable').DataTable({
                //language: {
                //    url: '//cdn.datatables.net/plug-ins/1.13.2/i18n/tr.json'
                //}
            });
        });
       
    }
    var tableAny2 = document.getElementById("my_DataTable_2");
    if (tableAny2) {
        $(document).ready(function () {
            $('#my_DataTable_2').DataTable({
                //language: {
                //    url: '//cdn.datatables.net/plug-ins/1.13.2/i18n/tr.json'
                //}
            });
        });

    }
}