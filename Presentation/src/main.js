import Vue from 'vue';
import App from './App.vue';
import router from './router';
import vuetify from './plugins/vuetify';
import Chartkick from 'vue-chartkick'
import Chart from 'chart.js'
import 'material-design-icons-iconfont/dist/material-design-icons.css';

Vue.use(Chartkick.use(Chart));
Vue.config.productionTip = false;
Vue.config.silent = true;

Vue.prototype.$globals = { 
  exemploHub: "https://localhost:44385/exemploHub",
  // endpoint: "https://localhost:44385/api/",
  // endpoint: "http://localhost/unicolabapi/api/",
  endpoint: " https://mettainfo.net/unicolabapi/api/",
  modulo: "123",
  headerPadrao: { 
    'Usuario': JSON.parse(localStorage.getItem("login"))?.id,
    'Content-Type': 'application/json' 
  },
  perfilAdmin: "6D672B19-8F61-413E-8277-3351DDDC5C56",
  perfilAluno: "5AB0D5A4-A8EE-4B3F-946F-1085D0BBE67D",
  perfilCoordenador: "F0292581-FD91-475F-B751-0604491EA3F5",
  perfilProfessor: "A8D3371E-84E4-4A29-8CF4-CA92AF25622F",
  areaTI: "08E0CC41-A608-4C54-8DF7-22F8B5688A96",
  areaSaude: "6217BA88-9FC9-4EAA-B6E6-CE4D950F19A9",
  areaEngenharia: "8D12BBE3-950E-487F-A24B-BFAB261D1059",
}

new Vue({
  router,
  vuetify,
  render: h => h(App)
}).$mount('#app');
