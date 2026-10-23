document.addEventListener('submit',function(e){const text=e.target.dataset.confirm;if(text&&!window.confirm(text))e.preventDefault();});
if(window.jQuery&&jQuery.validator){jQuery.validator.methods.number=function(value,element){return this.optional(element)||/^-?(?:\d+|\d+[,.]\d+)$/.test(value);};jQuery.validator.methods.range=function(value,element,param){const n=Number(value.replace(',','.'));return this.optional(element)||(n>=param[0]&&n<=param[1]);};}

if(window.jQuery&&jQuery.validator){jQuery.validator.addMethod("tien",function(value,element){return this.optional(element)||/^\d+(?:[,.]\d{1,2})?$/.test(value);});jQuery.validator.unobtrusive.adapters.addBool("tien");}
