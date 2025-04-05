function loadComments(projectId) {
    $.ajax({
        method: 'GET',
        url:'/ProjectManagement/ProjectComment/GetComments?projectId='+projectId,
        success:function(data){
            var commentsHtml ='';
            for (var i = 0; i < data.length; i++) {
                commentsHtml+='<div class="comments">';
                commentsHtml+='<p>'+data[i].content+'</p>'
                commentsHtml+='<span>Posted on: '+ new Date(data[i].datePosted).toLocaleDateString()+'</span>';
                commentsHtml+='</div><br>';
            }
            $('#commentsList').html(commentsHtml);
        }
    });
}
$(document).ready(function(){
    //loadComments- Call GetComments
    var projectId=$('#projectComments input[name="ProjectId"]').val();
    loadComments(projectId);
    
    //submit event for new comment (AddComment)
    $('#addCommentForm').submit(function(evt){
        //Stop default form submission
        evt.preventDefault();
        var formData={
            ProjectId:projectId,
            Content:$('#projectComments textarea[name="content"]').val()
        };
        $.ajax({
            url:'/ProjectManagement/ProjectComment/AddComment',
            method:'POST',
            contentType:'application/json',
            data: JSON.stringify(formData),
            success:function(response){
                if(response.success){
                    $('#projectComments textarea[name="content"]').val('')//clear new comment from form textarea
                    loadComments(projectId);
                }else{
                    alert(response.message);
                }
            },
            error:function(xhr, status, error) {
                alert("Error: "+xhr.responseText);
            }
        })
    })
});