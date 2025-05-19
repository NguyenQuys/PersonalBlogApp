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

            li.innerHTML = `<div class="row">
                                <div class="col-9">
                                    <strong class="username-response">${userName}</strong>: ${commentContent}
                                    <span class="text-muted" style="float:right;">${createdDate}</span>
                                </div>
                                <div class="col-3 d-flex">
                                        <form method="post" action="/Comments/Delete">
                                            <input type="hidden" name="blogId" value="${result.result.blogId}">                                            
                                            <input type="hidden" name="commentId" value="${result.result.id}">
                                            <button type="submit" class="btn btn-danger">Delete</button>
                                        </form>
                                        <span class="mx-3"> </span>
                                    <button type="button" onclick="OpenReplyInput('${result.result.id}', '${commentContent}')" class="btn btn-primary">Reply</button>
                                </div>
                            </div>
                            `

            document.getElementById('comment-list').appendChild(li);

            document.getElementById('comment-input').value = '';

            //// create reply btn
            //const replyButton = document.createElement('a');
            //replyButton.innerHTML = 'Reply';
            //replyButton.href = '#';
            //replyButton.className = 'reply-btn me-3';
            //replyButton.style.float = 'right';
            //replyButton.onclick = function (e) {
            //    e.preventDefault();
            //    // Call your reply input logic here, e.g.:
            //    // OpenReplyInput(result.result.id, commentContent);
            //};

            //li.appendChild();

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

//// Example SendReply function (implement server call as needed)
//async function SendReply(parentCommentId) {
//    const replyContent = document.getElementById('reply-input').value;
//    // TODO: Implement AJAX call to send reply to server
//    console.log(`Reply to comment ${parentCommentId}: ${replyContent}`);
//    // Remove input after sending
//    const inputDiv = document.getElementById('active-reply-input');
//    if (inputDiv) inputDiv.remove();
//}


