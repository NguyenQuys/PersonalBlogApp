async function Register() {
    const formData = new FormData();
    const file = document.getElementById("avatar-register").files[0];
    console.log("Selected file:", file); // ✅ Kiểm tra file có chọn chưa

    formData.append("Email", $('#email-register').val());
    formData.append("UserName", $('#username-register').val());
    formData.append("PasswordHash", $('#password-register').val());
    formData.append("AvatarUrl", file);
    console.log(formData);
    try {
        const response = await fetch("/Auth/Register", {
            method: 'POST',
            body: formData
        });

        if (response.ok) {
            const result = await response.json(); 
            toastr.success(result.message);
        } else {
            console.error("Server error:", response.statusText);
        }
    } catch (err) {
        console.error(err);
    }
}
