
window.bletrisInterop = {
  setFocus: function (id) {
      try {
          document.getElementById(id).focus();
      } catch {
          return false;
      }
      return true;
  }
};
