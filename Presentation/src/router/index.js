import Vue from 'vue'
import VueRouter from 'vue-router'

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'Login',
    component: () => import('../views/login.vue')
  },
  {
    path: '/modulos',
    name: 'modulos',
    component: () => import('../views/modulos.vue')
  },
  {
    path: '/duvidas',
    name: 'duvidas',
    component: () => import('../views/duvidas.vue')
  },
  {
    path: '/contatos',
    name: 'contatos',
    component: () => import('../views/contatos.vue')
  },
  {
    path: '/links-importantes',
    name: 'links-importantes',
    component: () => import('../views/links-importantes.vue')
  },
  {
    path: '/oportunidades',
    name: 'oportunidades',
    component: () => import('../views/oportunidades.vue')
  },
  {
    path: '/eventos',
    name: 'eventos',
    component: () => import('../views/eventos.vue')
  },
  {
    path: '/perfil',
    name: 'perfil',
    component: () => import('../views/perfil.vue')
  },
  {
    path: '/dados-mestre',
    name: 'dados-mestre',
    component: () => import('../views/dados-mestre/dados-mestre.vue')
  },
  {
    path: '/dados-mestre/usuarios',
    name: 'usuarios',
    component: () => import('../views/dados-mestre/usuarios.vue')
  },
  {
    path: '/dados-mestre/gestao-links-importantes',
    name: 'gestao-links-importantes',
    component: () => import('../views/dados-mestre/gestao-links-importantes.vue')
  },
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router
