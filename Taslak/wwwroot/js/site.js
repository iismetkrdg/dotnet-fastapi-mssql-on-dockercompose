function toggleMenuProfile(keySwitch) {
    if (keySwitch == "change" ) {
        // see the changePass id item
        document.getElementById("changePass").style.display = "block";
        // hide the profileData id item
        document.getElementById("profileData").style.display = "none";
        // hide the deleteAcc id item
        document.getElementById("deleteAcc").style.display = "none";
    }
    if (keySwitch == "delete") {
        // see the profileData id item
        document.getElementById("deleteAcc").style.display = "block";
        // hide the changePass id item
        document.getElementById("changePass").style.display = "none";
        // hide the deleteAcc id item
        document.getElementById("profileData").style.display = "none";
    }
}
function ToggMyPW() {
    var x = document.getElementById("newP");
    var y = document.getElementById("newPC");
    if (x.type === "password" && y.type === "password") {
        x.type = "text";
        y.type = "text";
    } else {
        x.type = "password";
        y.type = "password";
    }
}
document.getElementById("chngFrm").addEventListener("submit", function (event) {
    event.preventDefault();
    ChangePassword();
});
function ChangePassword() {
    var b = document.getElementById("sbmtbtnpwd");
    var s1 = document.getElementById("span-1-pwd");
    var s2 = document.getElementById("span-2-pwd");
    var m1 = document.getElementById("messpwd");
    s1.hidden = false;
    s2.textContent = "Changing...";
    b.disable = true;
    var x = document.getElementById("newP");
    var y = document.getElementById("newPC");
    // check if the password is the same
    if (x.value != y.value) {
        alert("Passwords do not match!");
        return false;
    }
    // check if the password is empty
    if (x.value == "" && y.value == "") {
        alert("Password fields are empty!");
        return false;
    }
    // check if the password is less than 6 characters
    if (x.value.length < 6 && y.value.length < 6) {
        alert("Password must be at least 6 characters long!");
        return false;
    }
    
    //send the data to the server to change the password
    $.ajax({
        type: "POST",
        url: "/Account/ChangePassword",
        data: { newP: x.value , newPC: y.value },
        success: function (data) {
            b.disable = false;
            s1.hidden = true;
            s2.innerText = "Change";
            m1.hidden = false;
            RedirectUsertoProfil();
        },
        error: function (data) {
            alert("Error while changing password!");
        }
    });
}
function RedirectUsertoProfil(){
    var t1 = document.getElementById("number-id-time");
    for (let i = 5; i > 0; i--) {
        setTimeout(function () {
            t1.innerText = i;
          if (i === 1) {
            window.location.href = "https://web.ismetkaradag.com/account/profil"
          }
        }, (5 - i) * 1000);
      }
}