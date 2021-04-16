var editor = null;

function getEditor(language) {
  $(document).ready(function () {
    if (editor == null) {
      editor = CodeMirror.fromTextArea
        (document.getElementById('editor'), {
          mode: language,
          autoRefresh: true,
          lineNumbers: true
        });
    }
  });
}

function setCode(code) {
  editor.getDoc().setValue(code);
  setTimeout(function () {
    editor.refresh();
  }, 10);
}

function getCode() {
  return editor.getValue();
}