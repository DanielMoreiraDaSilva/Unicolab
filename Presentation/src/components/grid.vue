<template>
  <div class="grid-component">
    <div class="mb-2 d-flex" id="filter-chips">
      <div v-for="(item, index) in chips" :key="index">
        <v-chip
          class="ma-1"
          close
          outlined
          color="#3b71fe"
          @click:close="removeChip(item)"
        >
          {{ item.descricao + ": " + item.valor }}
        </v-chip>
      </div>
    </div>
    <div
      class="d-flex justify-space-between"
      id="grid-action-buttons"
      v-if="filterType == 'search'"
    >
      <div class="d-flex">
        <v-btn
          rounded
          text
          dense
          class="pl-0 pr-1 mr-2"
          color="#3b71fe"
          @click="insertItem()"
          :disabled="editing || hasSelectedItem"
          v-if="!readonly && !hideInsert"
        >
          <v-icon>mdi-plus</v-icon>
          Inserir
        </v-btn>
        <v-btn
          rounded
          text
          dense
          class="pl-0 pr-1 mr-2"
          color="#3b71fe"
          :disabled="editing || !hasSelectedItem"
          @click="editItem()"
          v-if="!readonly && !hideEdit"
        >
          <v-icon>mdi-pencil-outline</v-icon>
          Editar
        </v-btn>
        <v-btn
          text
          dense
          class="pl-0 pr-1 mr-2"
          color="#3b71fe"
          :disabled="selectedItems == 0"
          @click="editItems()"
          v-if="!readonly && hasMultipleEdit"
        >
          <v-icon>{{ multipleEditIcon }}</v-icon>
          {{ multipleEditDescription }}
        </v-btn>
        <v-btn
          text
          dense
          class="pl-0 pr-1 mr-2"
          color="red"
          :disabled="selectedItems == 0"
          @click="excludeItems()"
          v-if="!readonly && hasExclude"
        >
          <v-icon>{{ multipleExcludeIcon }}</v-icon>
          {{ multipleExcludeDescription }}
        </v-btn>
        <div v-for="(item, index) in customButtonsList" :key="index">
          <v-btn
            text
            dense
            class="pl-1 pr-1 mr-2"
            :color="item.color"
            @click="customButtonClick(item)"
            :disabled="
              (item.enableIfHasSelectedItems ? selectedItems == 0 : false) ||
              (item.enableIfHasSingleItem ? selectedItems != 1 : false)
            "
            v-if="hasCustomButtons"
          >
            <v-icon>{{ item.customButtonIcon }}</v-icon>
            {{ item.customButtonDescription }}
          </v-btn>
        </div>
        <v-btn
          text
          dense
          class="pl-0 pr-1 mr-2"
          color="#3b71fe"
          @click="exportExcel()"
          v-if="!hideExport"
        >
          <v-icon>mdi-file-excel-outline</v-icon>
          Exportar
        </v-btn>
        <v-btn
          text
          dense
          class="pl-0 pr-1 mr-2"
          color="#3b71fe"
          :disabled="editing || !hasSelectedItem"
          v-if="gridType != 'responsive'"
          @click="saveItem()"
        >
          <v-icon>mdi-content-save-outline</v-icon>
          Atualizar
        </v-btn>
        <v-btn
          text
          dense
          class="pl-0 pr-1 mr-2"
          color="red"
          :disabled="editing || !hasSelectedItem"
          v-if="gridType != 'responsive'"
        >
          <v-icon>mdi-close</v-icon>
          Cancelar
        </v-btn>
        <span v-if="selectedItems >= 1" class="pt-1">
          {{
            selectedItems == 1
              ? selectedItems + " item selecionado"
              : selectedItems + " itens selecionados"
          }}
        </span>
      </div>
      <div
        class="mb-2 search-bar-container"
        id="search-bar-container"
        v-if="!hideFilters"
      >
        <div class="search-bar">
          <v-menu
            offset-y
            left
            :close-on-content-click="false"
            v-model="chevronSearch.opened"
          >
            <template v-slot:activator="{ on }">
              <v-icon
                :color="chevronSearch.opened ? '#3b71fe' : 'gray'"
                :class="
                  chevronSearch.opened ? 'mt-2 mr-2 rotate-180' : 'mt-2 mr-2'
                "
                v-on="on"
                >mdi-chevron-down</v-icon
              >
            </template>
            <v-card class="max-height-search-box">
              <v-card-text>
                <div
                  v-for="(item, index) in getSearchableFields()"
                  :key="index"
                >
                  <v-checkbox
                    class="mt-2 mb-2"
                    v-model="item.selecionado"
                    :label="item.descricao"
                    :value="item.selecionado"
                    hide-details
                    color="#3b71fe"
                  ></v-checkbox>
                </div>
              </v-card-text>
            </v-card>
          </v-menu>
          <v-text-field
            label="Procurar"
            hide-details
            append-icon="mdi-magnify"
            clearable
            v-on:keyup.esc="removeFocus($event)"
            v-on:keyup.enter="executeSearch($event)"
            @click:append="executeSearch($event)"
            v-model="search"
            dense
            color="#3b71fe"
            :autofocus="autofocusSearch"
          ></v-text-field>
        </div>
      </div>
    </div>
    <div class="d-flex justify-space-between">
      <div class="d-flex align-center">
        <div
          v-if="minimizedItemList.length > 0"
          class="minimized-items-menu pl-1 pr-1"
        >
          <v-menu
            offset-y
            right
            :close-on-content-click="true"
            v-model="showMinimizedItems"
            max-width="100vw"
          >
            <template v-slot:activator="{ on }">
              <div v-on="on" class="pa-0 minimized-items-button-container">
                <v-icon color="#3b71fe">mdi-format-list-numbered</v-icon>
                {{ minimizedItemList.length
                }}{{
                  minimizedItemList.length == 1
                    ? " item minimizado"
                    : " itens minimizados"
                }}
              </div>
            </template>

            <v-list>
              <v-list-item
                v-for="(item, index) in minimizedItemList"
                :key="index"
                @click="selectMinimizedItem(item)"
              >
                <v-list-item-title class="gray--text text--darken-1">
                  {{ item.titulo }}</v-list-item-title
                >
              </v-list-item>
            </v-list>
          </v-menu>
        </div>
        <v-btn
          text
          dense
          class="pl-0 pr-1 mr-2"
          color="primary"
          @click="insertItem()"
          :disabled="editing || hasSelectedItem"
          v-if="filterType == 'popup' && !hideInsert"
        >
          <v-icon>mdi-plus</v-icon>
          Inserir
        </v-btn>
        <v-btn
          text
          dense
          class="pl-0 pr-1 mr-2"
          color="primary"
          :disabled="editing || !hasSelectedItem"
          @click="editItem()"
          v-if="filterType == 'popup' && !hideEdit"
        >
          <v-icon>mdi-pencil-outline</v-icon>
          Editar
        </v-btn>
        <span v-if="customHeaderLabel">
          {{ customHeaderLabel }}
        </span>
      </div>
      <div v-if="filterType == 'popup' && !hideFilters">
        <v-btn text dense class="pa-0" color="#3b71fe" @click="openFilter()">
          <v-icon>mdi-magnify</v-icon>
          Filtrar
        </v-btn>
      </div>
    </div>

    <div
      :class="
        gridOverflow == 'vertical'
          ? 'grid-container'
          : 'grid-container-horizontal'
      "
      id="grid-container"
    >
      <table
        :class="!hasFixedColumns ? 'grid' : 'grid grid-with-fixed-column'"
        v-columns-resizable="gridResizable ? true : false"
      >
        <thead>
          <tr>
            <th v-if="fixedFields">
              <div class="first-column-container">
                <div
                  :class="
                    field.tipo == 'checkbox'
                      ? 'pa-0 d-flex justify-center'
                      : 'pl-2 pr-2 d-flex justify-space-between'
                  "
                  v-for="(field, innerIndex) in fixedFields"
                  :key="innerIndex"
                  :style="'width: ' + field.largura + 'px'"
                >
                  <span
                    v-if="field.tipo != 'checkbox'"
                    class="span-fixed-field-title"
                    @click="setOrder(field, innerIndex, true)"
                  >
                    {{ field.descricao }}
                  </span>
                  <v-icon
                    color="grey lighten-2"
                    v-if="field.tipo != 'checkbox' && field.ordenado != null"
                  >
                    {{
                      field.ordenado == true ? "mdi-arrow-up" : "mdi-arrow-down"
                    }}</v-icon
                  >
                  <v-checkbox
                    v-if="field.tipo == 'checkbox'"
                    class="pa-0 mt-0"
                    v-model="selectAll"
                    :label="null"
                    hide-details
                    @change="selectAllItems()"
                    dense
                  ></v-checkbox>
                </div>
              </div>
            </th>
            <th
              scope="col"
              :class="
                field.tipo == 'botao' || field.tipo == 'menu'
                  ? 'td-button'
                  : 'th-title pl-2 pr-2'
              "
              v-for="(field, innerIndex) in fields"
              :key="innerIndex"
            >
              <div class="title-container">
                <span
                  v-if="field.tipo != 'checkbox'"
                  :class="
                    field.tipo == 'botao' ||
                    field.tipo == 'menu' ||
                    disableOrder
                      ? 'title-without-click'
                      : 'title-click'
                  "
                  @click="setOrder(field, innerIndex, false)"
                  >{{ field.descricao }}</span
                >
                <v-checkbox
                  v-if="field.tipo == 'checkbox'"
                  class="pa-0 mt-0"
                  v-model="selectAll"
                  :label="null"
                  hide-details
                  @change="selectAllItems()"
                  dense
                ></v-checkbox>
                <div>
                  <v-icon color="grey lighten-2" v-if="field.ordenado != null">
                    {{
                      field.ordenado == true ? "mdi-arrow-up" : "mdi-arrow-down"
                    }}</v-icon
                  >
                  <v-menu
                    offset-y
                    left
                    v-if="field.filtravel"
                    :close-on-content-click="false"
                  >
                    <template v-slot:activator="{ on }">
                      <v-icon
                        :color="field.filtrado ? '#3b71fe' : 'grey lighten-2'"
                        v-on="on"
                        :id="'menu-filtro-' + field.valor"
                        >{{
                          field.filtrado ? "mdi-filter-menu" : "mdi-filter"
                        }}</v-icon
                      >
                    </template>
                    <v-card class="filter-card">
                      <v-card-text class="pb-0">
                        <div class="filter-options">
                          <div v-if="field.tipo != 'lista'">
                            <div
                              v-for="(
                                itemLista, indiceItemLista
                              ) in field.lista"
                              :key="indiceItemLista"
                            >
                              <v-checkbox
                                class="mt-2 mb-2"
                                v-model="itemLista.selecionado"
                                :label="itemLista.descricao"
                                :value="itemLista.itemValue"
                                hide-details
                                @change="updateFieldModel(field)"
                                color="#3b71fe"
                              ></v-checkbox>
                            </div>
                          </div>
                          <div v-if="field.tipo == 'lista'">
                            <v-radio-group
                              v-model="field.modelFiltro"
                              class="pa-0"
                            >
                              <v-radio
                                v-for="itemLista in field.lista"
                                :key="itemLista"
                                :label="itemLista.text"
                                :value="itemLista.value"
                                color="#3b71fe"
                              ></v-radio>
                            </v-radio-group>
                          </div>
                        </div>
                        <div>
                          <v-btn
                            :disabled="!field.filtrado"
                            class="btn-clear-filter"
                            color="grey darken-2"
                            text
                            right
                            @click="clearFilter(field)"
                          >
                            <v-icon color="red lighten-1" left
                              >mdi-filter-remove</v-icon
                            >Limpar filtro de "{{ field.descricao }}"
                          </v-btn>
                        </div>
                        <v-divider class="mt-6"></v-divider>
                      </v-card-text>
                      <v-card-actions>
                        <v-spacer></v-spacer>
                        <v-btn
                          color="red"
                          text
                          right
                          @click="closeFilter(field)"
                        >
                          <v-icon>mdi-close</v-icon
                          >Cancelar
                        </v-btn>
                        <v-btn
                          color="#3b71fe"
                          text
                          @click="applyFilter(field)"
                        >
                          <v-icon>mdi-check</v-icon>
                          Aplicar
                        </v-btn>
                      </v-card-actions>
                    </v-card>
                  </v-menu>
                </div>
              </div>
            </th>
          </tr>
        </thead>
        <tbody
          @focusout="handleFocusOut"
          :id="!gridTableId ? 'grid-table' : gridTableId"
        >
          <tr
            v-for="(item, index) in list"
            :key="index"
            :class="item.selecionado ? 'selected-item' : ''"
          >
            <td
              v-if="fixedFields"
              @click="selectRow(item)"
              @dblclick="editRow(item, index, innerIndex)"
            >
              <div class="first-column-container">
                <div
                  class="pl-2 pr-2"
                  v-for="(field, innerIndex) in fixedFields"
                  :key="innerIndex"
                  :style="'width: ' + field.largura + 'px'"
                >
                  <span
                    v-if="field.tipo != 'checkbox'"
                    class="span-fixed-field"
                  >
                    {{ item[field.valor] }}
                  </span>
                  <v-checkbox
                    v-if="field.tipo == 'checkbox'"
                    class="pa-0 mt-0"
                    v-model="item.selecionado"
                    :label="null"
                    hide-details
                    dense
                    @change="selectSingleItem(item)"
                  ></v-checkbox>
                </div>
              </div>
            </td>
            <td
              @click="selectRow(item)"
              @dblclick="editRow(item, index, innerIndex)"
              v-for="(field, innerIndex) in fields"
              :key="innerIndex"
              :class="selectTdClass(field, item)"
            >
              <div
                class="text-center"
                v-if="field.tipo == 'icone' && item[field.valor]"
              >
                <v-icon :color="item[field.valor].color">
                  {{ item[field.valor].mdiIcon }}
                </v-icon>
              </div>
              <v-icon
                class="td-custom-icon"
                v-if="field.tipo == 'menu' && item.iconePersonalizado"
                >{{ item.iconePersonalizado }}</v-icon
              >
              <v-checkbox
                v-if="field.tipo == 'checkbox'"
                class="pa-0 mt-0"
                v-model="item.selecionado"
                :label="null"
                hide-details
                dense
                @change="selectSingleItem(item)"
              ></v-checkbox>
              <v-btn
                icon
                color="#3b71fe"
                v-on="on"
                @click="buttonClick(item)"
                v-if="field.tipo == 'botao'"
              >
                <v-icon>{{ field.valor }}</v-icon>
              </v-btn>

              <v-menu
                offset-y
                right
                :close-on-content-click="true"
                v-model="item.exibirMenu"
                v-if="field.tipo == 'menu'"
                class="text-center"
              >
                <template v-slot:activator="{ on }">
                  <v-icon v-on="on" color="#3b71fe">{{ field.valor }}</v-icon>
                </template>

                <v-list>
                  <div v-if="!hasCustomOptionsItem">
                    <v-list-item
                      v-for="(opcao, index) in field.opcoesMenu"
                      :key="index"
                      @click="optionButtonClick(item, opcao)"
                    >
                      <v-list-item-title :class="opcao.classe">
                        <v-icon left :class="opcao.classe">{{
                          opcao.icone
                        }}</v-icon>
                        {{ opcao.descricao }}</v-list-item-title
                      >
                    </v-list-item>
                  </div>
                  <div v-if="hasCustomOptionsItem">
                    <v-list-item
                      v-for="(opcao, index) in item.opcoesMenu"
                      :key="index"
                      @click="optionButtonClick(item, opcao)"
                    >
                      <v-list-item-title :class="opcao.classe">
                        <v-icon left :class="opcao.classe">{{
                          opcao.icone
                        }}</v-icon>
                        {{ opcao.descricao }}</v-list-item-title
                      >
                    </v-list-item>
                  </div>
                </v-list>
              </v-menu>

              <div
                :class="
                  (!field.editavel || !item.editando) && field.tipo != 'icone'
                    ? 'grid-text-div'
                    : 'hide'
                "
                :style="item.cor ? 'color: ' + item.cor + ' !important;' : ''"
              >
                {{ item[field.valor] }}
              </div>
              <input
                type="text"
                v-model="item[field.valor]"
                :class="field.editavel && item.editando ? 'edit-field' : 'hide'"
                :id="'item-field-' + index + '-' + innerIndex"
              />
            </td>
          </tr>
          <tr v-if="list.length == 0">
            <td
              :colspan="fields.length + 1"
              class="text-center red--text"
              style="position: relative"
            >
              <div class="no-content-container">Nenhum registro encontrado</div>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>
<script>
export default {
  props: [
    "fields",
    "list",
    "filters",
    "defaultInsertObject",
    "gridType",
    "filterType",
    "gridOverflow",
    "minimizedItemList",
    "readonly",
    "hasFixedColumns",
    "fixedFields",
    "hideFilters",
    "hideExport",
    "gridTableId",
    "hasExclude",
    "defaultMenuSearchObject",
    "searchableFields",
    "autofocusSearch",
    "hasMultipleEdit",
    "multipleEditIcon",
    "multipleEditDescription",
    "hideInsert",
    "hideEdit",
    "hasCheckbox",
    "multipleExcludeIcon",
    "multipleExcludeDescription",
    "hasCustomButtons",
    "customButtonsList",
    "noContentLeft",
    "hasCustomOptionsItem",
    "customHeaderLabel",
    "disableOrder",
    "gridResizable"
  ],
  data() {
    return {
      editing: false,
      hasSelectedItem: false,
      chevronSearch: {
        opened: false,
      },
      search: null,
      chips: [],
      howMinimizedItems: false,
      showMinimizedItems: false,
      selectAll: false,
      selectedItems: 0,
    };
  },
  methods: {
    getSearchableFields() {
      if (this.searchableFields) return this.searchableFields;
      else if (this.fixedFields)
        return this.fixedFields.filter((x) => x.tipo != "checkbox");
      else
        return this.fields.filter((x) => x.tipo != "botao" && x.tipo != "menu");
    },
    async selectAllItems() {
      this.list.forEach((x) => {
        x.selecionado = this.selectAll;
      });
      this.selectedItems = this.list.filter((x) => {
        return x.selecionado;
      }).length;
      this.hasSelectedItem = false;
    },
    selectSingleItem(item) {
      if (this.selectAll && item.selecionado == false) this.selectAll = false;
      else {
        const notSelected = this.list.filter((x) => {
          return !x.selecionado;
        });
        if (notSelected.length == 0) this.selectAll = true;
      }
      this.selectedItems = this.list.filter((x) => {
        return x.selecionado;
      }).length;
      if (this.selectedItems == 1) {
        this.hasSelectedItem = true;
      } else {
        this.hasSelectedItem = false;
      }
    },
    selectTdClass(field, item) {
      if (field.tipo == "botao" || field.tipo == "menu") return "td-button";
      if (
        (!field.editavel || !item.editando) &&
        this.gridOverflow == "vertical"
      )
        return "pl-2 pr-2 pt-1 pb-1";
      if (
        (!field.editavel || !item.editando) &&
        this.gridOverflow == "horizontal"
      )
        return "pl-2 pr-2 pt-1 pb-1 no-wrap";

      return "";
    },
    removeChip(item) {
      this.filters.pagina = 1;
      const index = this.chips.indexOf(item);
      this.chips.splice(index, 1);
      this.filters[item.campo] = null;
      this.$emit("listarItens", this.filters);
      this.selectedItems = 0;
      this.selectAll = false;
      this.hasSelectedItem = false;
    },
    selectRow(item) {
      if (!this.hasCheckbox) {
        this.list.forEach((x) => {
          if (x != item) {
            x.selecionado = false;
          }
        });
        item.selecionado = true;
        this.hasSelectedItem = true;
      }
    },
    editRow(item, index, innerIndex) {
      if (this.gridType != "responsive") {
        item.editando = true;
        let itemField = document.getElementById(
          "item-field-" + index + "-" + innerIndex
        );
        setTimeout(() => {
          itemField.focus();
        }, 100);
      } else {
        this.$emit("selecionarItem", item);
      }
    },
    updateFieldModel(field) {
      field.modelFiltro = field.lista
        .filter((x) => {
          return x.selecionado == true;
        })
        .map((x) => {
          return x.id;
        });
    },
    menuButtonClick(field) {
      const menuButton = document.getElementById("menu-filtro-" + field.valor);
      menuButton.click();
      menuButton.blur();
    },
    closeFilter(field) {
      this.menuButtonClick(field);
      field.modelFiltro = field.modelUltimoValor;
    },
    clearFilter(field) {
      field.filtrado = false;
      field.modelFiltro = null;
      field.lista.forEach((x) => {
        x.selecionado = false;
      });
      this.filters[field.valorFiltroEmLista] = field.modelFiltro;
      this.menuButtonClick(field);
      this.$emit("listarItens", this.filters);
    },
    applyFilter(field) {
      this.menuButtonClick(field);
      field.modelUltimoValor = field.modelFiltro;
      this.filters[field.valorFiltroEmLista] = field.modelFiltro;
      field.filtrado =
        field.tipo == "lista"
          ? field.modelFiltro != null
          : field.modelFiltro.length != 0;
      this.$emit("listarItens", this.filters);
    },
    setOrder(field, index, fixed) {
      if (this.hasFixedColumns && !fixed)
        index = index + this.fixedFields.length;

      if (field.tipo == "botao" || field.tipo == "menu" || this.disableOrder)
        return;
      this.filters.pagina = 1;
      this.fields.forEach((x) => {
        if (x != field) {
          x.ordenado = null;
        }
      });
      this.fixedFields?.forEach((x) => {
        if (x != field) {
          x.ordenado = null;
        }
      });
      if (field.ordenado == null) {
        field.ordenado = true;
        this.filters.ordenarPor = index;
        this.filters.ordem = 0;
      } else if (field.ordenado == true) {
        field.ordenado = false;
        this.filters.ordenarPor = index;
        this.filters.ordem = 1;
      } else {
        field.ordenado = null;
        this.filters.ordenarPor = 0;
        this.filters.ordem = 0;
      }
      this.selectedItems = 0;
      this.selectAll = false;
      this.hasSelectedItem = false;
      this.$emit("listarItens", this.filters);
    },
    async exportExcel() {
      this.$emit("exportarExcel", this.filters);
    },
    handleFocusOut(event) {
      if (!this.hasCheckbox) {
        const gridId = !this.gridTableId
          ? "#grid-table"
          : "#" + this.gridTableId;
        var gridTable = document.querySelector(gridId);
        if (!gridTable?.contains(event.target)) {
          this.list.forEach((x) => {
            x.selecionado = false;
          });
          this.hasSelectedItem = false;
        }
      }
    },
    async saveItem() {
      this.$emit("salvarItem");
    },
    openFilter() {
      this.$emit("abrirFiltro");
    },
    editItem() {
      if (this.gridType != "responsive") {
        console.log("editando diretamente no grid");
      } else {
        const itemSelecionado = this.list.filter((x) => {
          return x.selecionado == true;
        })[0];
        this.$emit("selecionarItem", itemSelecionado);
      }
    },
    insertItem() {
      if (this.gridType != "responsive") {
        this.editing = true;
        let newItem = {};
        newItem = Object.assign(this.defaultInsertObject, newItem);
        this.list.unshift(newItem);
        this.editRow(newItem, 0, 0);
      } else {
        this.$emit("selecionarItem", this.defaultInsertObject);
      }
    },
    removeFocus(event) {
      event.target.blur();
    },
    executeSearch(event) {
      this.filters.pagina = 1;
      if (this.search) {
        const selectedItems = [];
        this.fields.forEach((x) => {
          if (x.selecionado) {
            this.includeNewChip(x);
            selectedItems.push(x);
          }
          x.selecionado = false;
        });
        if (this.fixedFields) {
          this.fixedFields.forEach((x) => {
            if (x.selecionado) {
              this.includeNewChip(x);
              selectedItems.push(x);
            }
            x.selecionado = false;
          });
        }
        if (this.searchableFields) {
          this.searchableFields.forEach((x) => {
            if (x.selecionado) {
              this.includeNewChip(x);
              selectedItems.push(x);
            }
            x.selecionado = false;
          });
        }
        if (selectedItems.length == 0) {
          if (this.defaultMenuSearchObject)
            this.includeNewChip(this.defaultMenuSearchObject);
          else this.includeNewChip({ descricao: "Todos", valor: "todos" });
        }

        this.search = null;
      }
      event.target.blur();
      this.selectedItems = 0;
      this.selectAll = false;
      this.hasSelectedItem = false;
    },
    includeNewChip(item) {
      const existingChip = this.chips.filter((y) => {
        return y.descricao == item.descricao;
      });
      if (existingChip.length > 0) {
        const index = this.chips.indexOf(existingChip[0]);
        this.chips[index] = {
          campo: item.valor,
          descricao: item.descricao,
          valor: this.search,
        };
      } else {
        this.chips.push({
          campo: item.valor,
          descricao: item.descricao,
          valor: this.search,
        });
      }
      this.filters[item.valor] = this.search;
      this.$emit("listarItens", this.filters);
    },
    buttonClick(item) {
      this.$emit("botaoClick", item);
    },
    optionButtonClick(item, opcao) {
      const result = {
        item: item,
        opcao: opcao,
      };
      this.$emit("botaoOpcaoClick", result);
    },
    selectMinimizedItem(item) {
      this.$emit("selecionarItemMinimizado", item);
    },
    excludeItems() {
      const ids = this.list
        .filter((x) => {
          return x.selecionado;
        })
        .map((x) => {
          return x.id;
        });
      this.$emit("excluirItens", ids);
    },
    editItems() {
      const ids = this.list
        .filter((x) => {
          return x.selecionado;
        })
        .map((x) => {
          return x.id;
        });
      this.$emit("editarItens", ids);
    },
    customButtonClick(item) {
      if (item.hasMultipleAction) {
        const ids = this.list
          .filter((x) => {
            return x.selecionado;
          })
          .map((x) => {
            return x.id;
          });
        item.ids = ids;
      }
      this.$emit("customButtonClick", item);
    },
    resetAfterExclude() {
      this.selectedItems = 0;
      this.selectAll = false;
      this.hasSelectedItem = false;
    },
    addCustomIcon(index, icone) {
      this.list[index].iconePersonalizado = icone;
      this.$forceUpdate();
    },
    removeCustomIcon(index) {
      this.list[index].iconePersonalizado = null;
      this.$forceUpdate();
    },
  },
};
</script>
<style scoped>
.grid-component {
  font-family: 'Poppins';
  color: #3b71fe;
  display: flex;
  flex-flow: column;
  z-index: 1;
  position: relative;
}
#filter-chips {
  display: flex;
  align-items: center;
  justify-content: flex-start;
  flex-wrap: wrap;
}
.grid-container {
  overflow-y: auto;
  overflow-x: hidden;
  z-index: 1;
  position: relative;
}
.grid-container-horizontal {
  overflow-y: auto;
  overflow-x: auto;
  z-index: 1;
}
.no-wrap {
  white-space: nowrap;
}

.grid {
  width: 100%;
  text-align: left;
  color: #3d3d3d;
  border-spacing: 0;
  border-collapse: collapse;
}

.grid thead {
  position: sticky;
  top: 0;
  z-index: 1;
  background: #fff;
}

.grid th {
  border-bottom: 1.5px solid rgba(0, 0, 0, 0.12);
  font-size: 1.2rem;
  padding-top: 8px;
  padding-bottom: 8px;
  font-weight: 500;
  color: #3b71fe;
}

.grid tr {
  border-bottom: 0.5px solid rgba(0, 0, 0, 0.12);
}

.grid tbody tr:hover {
  background-color: #eee;
}

.grid tbody tr:hover .first-column-container {
  background-color: #eee;
}

.grid tbody tr td {
  height: 35px;
  font-size: 1.0rem;
}

.title-container {
  display: flex;
  justify-content: space-between;
}
.title-container span {
  white-space: nowrap;
}
.title-container div {
  width: max-content;
  text-align: right;
}

.search-bar-container {
  display: flex;
  justify-content: flex-end;
}

.rotate-180 {
  transform: rotate(-180deg);
}

.edit-field {
  height: 26px;
  width: calc(100% - 8px);
  margin: 0;
  color: #3d3d3d;
  padding: 0 4px;
  margin: 4px;
}

.edit-field:focus {
  outline: 1px solid rgba(0, 0, 0, 0.3);
  border-radius: 2px;
}

.edit-field:focus-visible {
  outline: 1px solid rgba(0, 0, 0, 0.3);
  border-radius: 2px;
}

.search-bar {
  display: flex;
  align-items: center;
  width: 300px;
  max-width: 300px;
}

.selected-item,
.selected-item .first-column-container {
  background-color: #dedede;
}

.selected-item:hover,
.selected-item:hover .first-column-container {
  background-color: #dedede !important;
}

.hide {
  display: none;
}

.title-click {
  cursor: pointer;
  user-select: none;
  -moz-user-select: none;
  -webkit-user-select: none;
}

.title-without-click {
  user-select: none;
  -moz-user-select: none;
  -webkit-user-select: none;
}

.filter-card {
  max-width: fit-content;
}

.btn-clear-filter {
  width: 100%;
}

.grid-text-div {
  user-select: none;
  -moz-user-select: none;
  -webkit-user-select: none;
}

.excluir-sub-item {
  cursor: pointer;
  margin-top: -12px;
}

.filter-options {
  max-height: 200px;
  overflow-y: auto;
  border: 1px solid #e4e4e4;
  padding: 0px 12px;
  border-radius: 5px;
}

.td-button {
  width: 60px !important;
  text-align: center;
}

.td-button .title-container {
  justify-content: center;
}

.th-title {
}

.minimized-items-menu {
  display: flex;
  align-items: center;
  cursor: pointer;
  color: #3b71fe;
  z-index: 1;
  border-radius: 3px;
}

.minimized-items-menu:hover {
  background-color: #3b71fe14;
}

.minimized-items-button-container {
  position: relative;
  height: 40px;
  vertical-align: middle;
  line-height: 40px;
}

.max-height-search-box {
  max-height: 400px;
  overflow: auto;
}

.no-content-container {
  /* position: fixed;
  margin-top: -10px;
  left: calc(50% - 115px);
  width: 230px;
  text-align: center;
  line-height: 20px; */
  margin-left: 14px;
  text-align: left;
}

@media (max-width: 600px) {
  .search-bar {
    align-items: center;
    display: flex;
    width: 100%;
    max-width: 100%;
  }
}

/* ------------------------------------- tabela com duplo scroll-------------------------------------  */
.grid-with-fixed-column {
  border-collapse: separate;
}

.grid-with-fixed-column tfoot,
.grid-with-fixed-column tfoot th,
.grid-with-fixed-column tfoot td {
  position: -webkit-sticky;
  position: sticky;
  bottom: 0;
  z-index: 4;
}

.grid-with-fixed-column td:first-child,
.grid-with-fixed-column th:first-child {
  position: -webkit-sticky;
  position: sticky;
  left: 0;
  z-index: 0;
  /* background-color: white; */
  border-right: 1.5px solid rgba(0, 0, 0, 0.12);
}
.grid-with-fixed-column thead th:first-child,
.grid-with-fixed-column tfoot th:first-child {
  z-index: 5;
  /* background-color: white; */
}

.grid-with-fixed-column th {
  font-size: 0.9rem !important;
}

.grid-with-fixed-column tbody tr td {
  font-size: 0.8rem !important;
}

.first-column-container {
  display: flex;
  align-items: center;
  justify-content: flex-start;
  background-color: white;
}

.first-column-container div {
  white-space: normal;
}

.span-fixed-field-title {
  cursor: pointer;
  user-select: none;
  -moz-user-select: none;
  -webkit-user-select: none;
}

.span-fixed-field {
  cursor: default;
  user-select: none;
  -moz-user-select: none;
  -webkit-user-select: none;
}

.td-custom-icon {
  font-size: 1.2rem;
  color: #aaa;
  margin-left: -18px;
}
</style>