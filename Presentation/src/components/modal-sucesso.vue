<template>
  <div>
    <v-row justify="center">
      <v-dialog content-class="rounded-xl" v-model="alertaSucesso" max-width="600px" persistent>
        <v-card rounded="xl">
          <v-toolbar dark color="green lighten-1">
            <v-toolbar-title class="headline">{{titulo}}</v-toolbar-title>
            <v-spacer></v-spacer>
            <v-btn icon dark @click="ocultarModal">
              <v-icon>mdi-close</v-icon>
            </v-btn>
          </v-toolbar>
          <v-card-text class="mt-4">
            <p class="title text-center">{{mensagem}}</p>
          </v-card-text>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn rounded x-large color="green lighten-1" @click="ocultarModal" text class="mr-4 mb-4"> <v-icon>mdi-check</v-icon> OK</v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
    </v-row>
  </div>
</template>
<script>
export default {
  props: ["mensagem", "alertaSucesso", "titulo"],
  methods: {
    ocultarModal() {
      this.$emit("ocultarSucesso");
    }
  }
};
</script>
<style scoped>
.v-card {
  border-radius: 0.5em;
}

.v-toolbar {
  border-radius: 0.5em;
}
</style>