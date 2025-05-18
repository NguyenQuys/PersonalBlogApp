async function AddComment() {
    const content = $('#comment-input').val();
    const blogId = $('#blog-id').val();

    const formData = new FormData();
    formData.append("Content", content);
    formData.append("BlogId", blogId);

    try {
        const response = await fetch("/Comments/Create", {
            method: 'POST',
            body: formData
        });

        const result = await response.json();
        if (result.status === 201) {

            const userName = "You";
            const commentContent = result.result.content;
            const createdDate = new Date(result.result.createdDate).toLocaleString();

            const li = document.createElement('li');
            li.className = 'list-group-item';
            li.innerHTML = `<strong class="username-response">${userName}</strong>: ${commentContent}
                <span class="text-muted" style="float:right;">${createdDate}</span>`;

            document.getElementById('comment-list').appendChild(li);

            document.getElementById('comment-input').value = '';
        } else {
            console.log('Failed to add comment: ' + (result.message || 'Unknown error'));
        }
    } catch (error) {
        console.error('Error:', error);
    }
}
