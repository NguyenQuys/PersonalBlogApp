async function DeleteUser(userId) {
    if (confirm('Are you sure you want to delete this user?')) {
        const response = await fetch(`/Users/${userId}`, {
            method: 'DELETE',
        });
        
        if (response.ok) {
            window.location.href = '/Users';
        } else {
            alert('Error deleting user.');
        }
    }
}

//async function DeleteUser(userId) {
//    if (confirm('Are you sure you want to delete this user?')) {
//        const response = await fetch(`/Users/${userId}`, {
//            method: 'DELETE',
//        });
//        // Log the raw response object
//        console.log(response);

//        // Try to read the response body as JSON (if your API returns JSON)
//        let data;
//        try {
//            data = await response.json();
//            console.log('Response body:', data);
//        } catch (e) {
//            // If not JSON, try as text
//            const text = await response.text();
//            console.log('Response text:', text);
//        }

//        if (response.ok) {
//            //window.location.href = '/Users';
//        } else {
//            alert('Error deleting user.');
//        }
//    }
//}
