let firstReplyTo = null;

async function AddComment() {
    const content = $('#comment-input').val();
    const blogId = $('#blog-id').val();

    const formData = new FormData();
    formData.append("Content", content);
    formData.append("BlogId", blogId);

    try {
        const response = await fetch("/Comments", {
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
            li.id = `comment-${result.result.id}`
            li.innerHTML = `<div class="row">
                                <div class="col-lg-10 col-md-10 col-sm-10 d-flex justify-content-between align-items-center">
                                    <span><strong class="username-response">${userName}</strong>: ${commentContent}</span>
                                    <span class="text-muted ms-2">${createdDate}</span>
                                </div>
                                <div class="col-lg-2 col-md-2 col-sm-2 d-flex justify-content-end">
                                        <form method="post" action="/Comments/Delete">
                                            <input type="hidden" name="blogId" value="${result.result.blogId}">                                            
                                            <input type="hidden" name="commentId" value="${result.result.id}">
                                            <button type="submit" class="btn btn-link">Delete</button>
                                        </form>
                                        <span class="mx-3"> </span>
                                    <button type="button" onclick="OpenReplyInput('${result.result.id}', '${commentContent}')" class="btn btn-link">Reply</button>
                                </div>
                            </div>
                            `

            document.getElementById('comment-list').appendChild(li);

            document.getElementById('comment-input').value = '';

        } else {
            console.log('Failed to add comment: ' + (result.message || 'Unknown error'));
        }
    } catch (error) {
        console.error('Error:', error);
    }
}

async function OpenReplyInput(parentCommentId, cmtContentParent) {
    const existingInput = document.getElementById('active-reply-input');
    if (existingInput) {
        existingInput.remove();
    }

    const divInput = document.createElement('div');
    divInput.className = 'input-group my-2';
    divInput.id = 'active-reply-input';
    divInput.innerHTML = `
        <input type="text" class="form-control" placeholder="Reply to ${cmtContentParent}..." id="reply-input">
        <button class="btn btn-primary" type="button" onclick="SendReply('${parentCommentId}')">Send</button>

    `;

    document.getElementById('comment-reply').appendChild(divInput);
}

async function SendReply(parentCommentId) {
    const blogId = $('#blog-id').val();
    const replyContent = $('#reply-input').val();

    const formData = new FormData();
    formData.append("Content", replyContent);
    formData.append("BlogId", blogId);
    formData.append('ParentCommentId', parentCommentId);

    const response = await fetch("/Comments", {
        method: 'POST',
        body: formData
    });

    if (response.ok) {
        const result = await response.json()
        const createdDate = new Date(result.result.createdDate).toLocaleString();

        let replyList = document.getElementById(`reply-list-${parentCommentId}`);

        if (!replyList) {
            replyList = document.createElement('ul');
            replyList.className = 'list-group mt-2 ms-4';
            replyList.id = `reply-list-${parentCommentId}`;

            const parentCommentElement = document.getElementById(`comment-${parentCommentId}`);
            parentCommentElement.appendChild(replyList);
        }

        const li = document.createElement('li');
        li.className = 'list-group-item';
        li.innerHTML = `
            <div>
                <strong class="username-response">${result.result.username}</strong>: ${result.result.content}
                <form method='post' class='d-inline' action='/Comments/Delete'>
                    <input type='hidden' name='blogId' value='${result.result.blogId}'>
                    <input type='hidden' name='commentId' value='${result.result.id}'>
                    <button type='submit' style='float:right' class='btn btn-link btn-sm ms-4'>Delete</button>
                </form>
                <span class="text-muted" style="float:right;">${createdDate}</span>
            </div>
        `;
        replyList.appendChild(li);

        // delete after send reply
        const inputDiv = document.getElementById('active-reply-input');
        if (inputDiv) inputDiv.remove();
    }
}



