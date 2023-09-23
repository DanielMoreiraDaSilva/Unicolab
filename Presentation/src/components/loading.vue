<template>
  <div class="loading-bg">
    <orbit-spinner :animation-duration="800" :size="95" color="#3b71fe" />
  </div>
</template>
<script>
import { OrbitSpinner   } from 'epic-spinners'
export default {
  components: {
    OrbitSpinner ,
  },
};
</script>
<style scoped>
.loading-bg {
  position: fixed;
  top: 0;
  left: 0;
  z-index: 1000;
  height: 100vh;
  width: 100vw;
  background-color: rgba(0, 0, 0, 0.7);
  display: flex;
  align-items: center;
  justify-content: center;
}
</style>